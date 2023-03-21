using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;
using System.Diagnostics;
using System;


namespace WPFToolkit.Drawing
{
    /// <summary>
    /// Helper class which contains general helper functions and properties.
    /// 
    /// Most functions in this class replace VisualCollection-derived class
    /// methods, because I cannot derive from VisualCollection.
    /// They make different operations with GraphicsBase list.
    /// </summary>
    static class HelperFunctions
    {
        public static bool hasSelected(DrawingCanvas drawingCanvas)
        {
            bool ok = false;
            foreach (GraphicsBase g in drawingCanvas.GraphicsList)
            {
                if (g.IsOption)
                {
                    ok = true;
                    return ok;
                }
            }
            return ok;
        }

        public static double calculateCos(Point from1,Point to1,Point from2,Point to2)
        {
            double x1 = to1.X - from1.X;
            double y1 = to1.Y - from1.Y;
            double x2 = to2.X - from2.X;
            double y2 = to2.Y - from2.Y;

            double numerator = x1 * x2 + y1 * y2;
            double denominator = Math.Sqrt(x1*x1 + y1*y1) * Math.Sqrt(x2*x2 + y2*y2);

            double ans = numerator / denominator;

            if (Math.Abs(ans) < 1 + Threshold.Esp)
            {
                return ans;
            }
            return Double.MaxValue;
        }
        /// <summary>
        /// ����������֮���б��
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double getRakeRatio(Point p1,Point p2)
        {
            if (p1.X - p2.X < Threshold.Esp)
            {
                return Threshold.Inf;
            }
            return (p1.Y - p2.Y)/(p1.X - p2.Y);
        }
        /// <summary>
        /// ��ȡ������֮������ĵ�
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static Point getCenterPoint(Point p1,Point p2)
        {
            double cx = (p1.X + p2.X) / 2;
            double cy = (p1.Y + p2.Y) / 2;
            Point centerPoint = new Point(cx,cy);
            return centerPoint;
        } 
        /// <summary>
        /// ��������֮������ƽ��
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static int CalcDistanceSquare(Point p1,Point p2)
        {
            if (p1 == null || p2 == null) return 0;
            return (int)((p1.X - p2.X)*(p1.X - p2.X)+(p1.Y - p2.Y)*(p1.Y - p2.Y));
        }

        /// <summary>
        /// �ж��Ƿ�������
        /// ��Ҫ�Ĳ�������ָ�������豸Id�õ�����һ����ָ����ʼ��
        /// </summary>
        /// <returns></returns>
        public static void IsGesture(DrawingCanvas drawingCanvas,TouchEventArgs e)
        {
            if (GestureData.Fingers < 2)
            {
                GestureData.IsGesture = false;
            }
            else if(GestureData.Fingers == 2)
            {
                int CurToBeginDistInFirstFinger = CalcDistanceSquare(GestureData.FirstFingerBeginPoint,GestureData.FirstFingerCurPoint);
                int firstFingerToSecFingerDist = CalcDistanceSquare(GestureData.FirstFingerBeginPoint, e.GetTouchPoint(drawingCanvas).Position);
                if (CurToBeginDistInFirstFinger < Threshold.CurToBeginMaxDistInFirstFinger 
                    && firstFingerToSecFingerDist < Threshold.FirstFingerToSecFingerMaxDist)//��һ����ָ�Ļ����ľ���С����ֵTHRESHOLD1 ���ң��ڶ�����ָ����һ����ָ����ʼ�����С����ֵTHRESHOLD2
                {
                    GestureData.IsGesture = true;
                }
            }
            else
            {
                //��ֹ�����ƵĹ����У�������ж����ָ�������أ����½����ƵĹ켣����
                /* if (GestureData.IsGesture)
                 {
                     GestureData.IsGesture = true;
                 }
                 else
                 {
                     GestureData.IsGesture = false;
                 }*/
                 GestureData.IsGesture = false;
            }
        }
        /// <summary>
        /// ��¼�±仯ʱ����ָ֮��Ķ�̬���룬�仯�㹻�󣨸�����ʾ��С��������ʾ�Ŵ�
        /// �Լ����ĵ��λ�ñ仯������˵�������Ų�����
        /// ��ƽ�Ƶ��ص�ո��෴��������ı仯�㹻���Լ���ָ֮��Ķ�̬����仯����(����Ч�����Ǻܺã�
        /// ����������һ���ص��жϣ�����ָ֮��б��)��
        /// ��ת���ص㣺���ı仯���󣬶�����ָ֮��Ķ�̬����Ҳ�仯����
        /// </summary>
        /// <param name="drawingCanvas"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static GestureId getGestureId(DrawingCanvas drawingCanvas,TouchEventArgs e)
        {
            TouchPoint touchPoint = e.GetTouchPoint(drawingCanvas);
            Point p = touchPoint.Position;
            //�õ���һ����ָ��ǰ��
            if (GestureData.FirstDeviceId == e.TouchDevice.Id)
            {
                GestureData.FirstFingerCurPoint = p;
            }
            //�õ��ڶ�����ָ��ǰ��
            if (GestureData.SecondDeviceId == e.TouchDevice.Id)
            {
                GestureData.SecFingerCurPoint = p;

                if (GestureData.IsSign)
                {
                    GestureData.PreFirstFingerPoint = GestureData.FirstFingerBeginPoint;
                    GestureData.PreSecFingerPoint = GestureData.SecFingerBeginPoint;
                    GestureData.PreDistInTwoFingers = GestureData.StartDistInTwoFingers;
                    GestureData.IsSign = false;
                }
                int curDistInTwoFingers = (int)CalcDistanceSquare(GestureData.FirstFingerCurPoint, GestureData.SecFingerCurPoint);
                //��ָ�䶯̬�ı仯����
                int deltaDist = curDistInTwoFingers - GestureData.PreDistInTwoFingers;
                Point curCenterPointInTwoFingers = getCenterPoint(GestureData.FirstFingerCurPoint, GestureData.SecFingerCurPoint);
                int centerDist = CalcDistanceSquare(curCenterPointInTwoFingers, GestureData.StartCenterPointInTwoFingers);
                
                if (deltaDist != 0)
                {
                    GestureData.PreFirstFingerPoint = GestureData.FirstFingerCurPoint;
                    GestureData.PreSecFingerPoint = GestureData.SecFingerCurPoint;
                    GestureData.PreDistInTwoFingers = curDistInTwoFingers;
                }

                if (GestureData.FirstFingerBeginPoint == GestureData.FirstFingerCurPoint || GestureData.SecFingerBeginPoint == GestureData.SecFingerCurPoint)
                {
                    return GestureId.None;
                }

                int k = deltaDist / Threshold.CurToBeginMaxDistInFirstFinger;
                double cosValue = calculateCos(GestureData.FirstFingerBeginPoint, GestureData.FirstFingerCurPoint, GestureData.SecFingerBeginPoint, GestureData.SecFingerCurPoint);
                
                if (cosValue - 0.9 > Threshold.Esp)
                {
                    double deltax = GestureData.FirstFingerCurPoint.X - GestureData.FirstFingerBeginPoint.X;
                    double deltay = GestureData.FirstFingerCurPoint.Y - GestureData.FirstFingerBeginPoint.Y;
                    //����15��Ϊ�˼����ٶ�
                    if (Math.Abs(deltax) > Math.Abs(deltay))
                    {
                        GestureData.Panx = deltax/20;
                        GestureData.Pany = 0;
                    }
                    else
                    {
                        GestureData.Pany = deltay/20;
                        GestureData.Panx = 0;
                    }
                    Debug.WriteLine("ƽ��");
                    return GestureId.PAN;
                }
                else//�Ŵ������С����ת
                {
                    double cosVal = calculateCos(GestureData.FirstFingerBeginPoint, GestureData.SecFingerBeginPoint, GestureData.FirstFingerCurPoint, GestureData.SecFingerCurPoint);

                    if (cosVal - 0.95 < Threshold.Esp)
                    {
                        GestureData.RorateCnt++;
                        if (GestureData.RorateCnt > 5)
                        {
                            
                            //�жϵ�ǰ��һָ��xС��λ�ã����ǵڶ�ָ��xС��λ��
                            if (GestureData.MaxPositionX)//��һ����ָΪ��Ĵָ
                            {
                                if (GestureData.SecFingerCurPoint.X > GestureData.SecFingerBeginPoint.X)//��ǰx����֮ǰ��x���ʾΪ˳ʱ��
                                {
                                    GestureData.RotateAngle += 1.0 / 6 * Math.PI;
                                }
                                else//����Ϊ��ʱ��(yֵ��С)
                                {
                                    GestureData.RotateAngle -= 1.0 / 6 * Math.PI;
                                }
                            }
                            else//����������ڶ�����ָΪ��Ĵָ
                            {
                                if (GestureData.SecFingerCurPoint.Y < GestureData.SecFingerBeginPoint.Y)//��ǰyС����֮ǰ��y���ʾΪ˳ʱ��
                                {
                                    //GestureData.RotateAngle += 0.1;
                                    GestureData.RotateAngle += 1.0 / 6 * Math.PI;
                                }
                                else//����Ϊ��ʱ��
                                {
                                    //GestureData.RotateAngle -= 0.1;
                                    GestureData.RotateAngle -= 1.0 / 6 * Math.PI;
                                }
                            }

                            GestureData.RorateCnt = 0;
                            Debug.WriteLine("��ת");
                            return GestureId.RORATE;
                        }
                    }
                    else
                    {
                        GestureData.ZoomCnt++;
                        if (GestureData.ZoomCnt > 3)
                        {
                            GestureData.ZoomCnt = 0;
                            if (k > 0 && GestureData.ZoomInScale - 3 < Threshold.Esp)
                            {
                                GestureData.ZoomOutScale = 1.0;
                                GestureData.ZoomInScale += 0.05;
                                GestureData.ZoomScale = GestureData.ZoomInScale;
                            }
                            if (k < 0 && GestureData.ZoomOutScale - 0.2 > Threshold.Esp)
                            {
                                GestureData.ZoomInScale = 1.0;
                                GestureData.ZoomOutScale -= 0.05;
                                GestureData.ZoomScale = GestureData.ZoomOutScale;
                            }
                            Debug.WriteLine("����");
                            return GestureId.ZOOM;
                        }
                        
                    }
                }
             
                /*if (Math.Abs(k) > Threshold.ScaleThreshold)
                {
                    GestureData.IsZoom = true;
                    if (k > 0 && GestureData.ZoomInScale - 3 < Threshold.Esp)
                    {
                        GestureData.ZoomOutScale = 1.0;
                        GestureData.ZoomInScale += 0.05;
                        GestureData.ZoomScale = GestureData.ZoomInScale;
                    }
                    if (k < 0 && GestureData.ZoomOutScale - 0.2 > Threshold.Esp)
                    {
                        GestureData.ZoomInScale = 1.0;
                        GestureData.ZoomOutScale -= 0.05;
                        GestureData.ZoomScale = GestureData.ZoomOutScale;
                    }
                    
                    //System.Diagnostics.Debug.WriteLine("����:{0},{1},{2},{3},{4}", curDistInTwoFingers, GestureData.PreDistInTwoFingers, deltaDist, centerDist, cosValue);
                    
                    return GestureId.ZOOM;
                }
                else
                {
                    if (deltaDist!=0 && GestureData.StartDistInTwoFingers < Threshold.PanMaxDist&&!GestureData.IsZoom)
                    {
                        double deltax = GestureData.FirstFingerCurPoint.X - GestureData.FirstFingerBeginPoint.X;
                        double deltay = GestureData.FirstFingerCurPoint.Y - GestureData.FirstFingerBeginPoint.Y;


                        if (Math.Abs(deltax) > Math.Abs(deltay))
                        {
                            GestureData.Panx = deltax;
                            GestureData.Pany = 0;
                        }
                        else
                        {
                            GestureData.Pany = deltay;
                            GestureData.Panx = 0;
                        }
                        //System.Diagnostics.Debug.WriteLine("ƽ��:{0},{1},{2},{3},{4}", curDistInTwoFingers, GestureData.PreDistInTwoFingers, deltaDist, centerDist,cosValue);
                        return GestureId.PAN;
                    }
                }*/

            }

            return GestureId.None;
        }

        /// <summary>
        /// ��ָup֮�󣬻�ԭGestureData�е����ݡ�
        /// </summary>
        public static void clear(DrawingCanvas drawingCanvas)
        {
            //��һ����ָ��Id
            GestureData.FirstDeviceId = -1;
            //�ڶ�����ָ��Id
            GestureData.SecondDeviceId = -1;
            //��һ����ָ��������ʼ��
            GestureData.FirstFingerBeginPoint = new Point(0, 0);
            //��һ����ָ��ǰ��λ��
            GestureData.FirstFingerCurPoint = new Point(0, 0);
            //�Ƿ�������
            GestureData.IsGesture = false;
            //Ĭ��Ϊ����¼�
            GestureData.IsTouch = false;

            //�Ƿ����һ����ָ��ʼ��ֵ����Щ����ֻ��ʼ��һ�Σ�
            GestureData.IsSignFoFingerOne = false;

            //�Ƿ���ڶ�����ָ��ʼ��ֵ����Щ����ֻ��ʼ��һ�Σ�
            GestureData.IsSignForFingerTwo = false;

            //��ָ�ʼ�ľ���
            GestureData.StartDistInTwoFingers = 0;
            //�ڶ�����ָ�Ĵ�������ʼ��
            GestureData.SecFingerBeginPoint = new Point(0, 0);
            //�ڶ�����ָ�ĵ�ǰλ��
            GestureData.SecFingerCurPoint = new Point(0, 0);
            //��ָ֮ǰ�ľ���
            GestureData.PreDistInTwoFingers = 0;
            //��preFirstFingerPoint��preSecFingerPoint��ֵ�ı�־��
            GestureData.IsSign = true;
            //�Ƿ��ǷŴ�
            GestureData.IsZoom = false;
            //��С�ߴ�
            GestureData.ZoomOutScale = 1.0;
            //�Ŵ�ߴ�
            GestureData.ZoomInScale = 1.0;
            //ʵ�ʳߴ�
            GestureData.ZoomScale = 1.0;
            //ƽ��x����
            GestureData.Panx = 0.0;
            //ƽ��y����
            GestureData.Pany = 0.0;
            //��ת
            //GestureData.RotateAngle = 0.0;
            //���Ʒ�����������ӿ��Ʒ����ٶ�
            GestureData.ZoomCnt = 0;
            //������ת��������ӿ��Ʒ����ٶ�
            GestureData.RorateCnt = 0;
            //�жϵ�ǰ��һָ��xС��λ�ã����ǵڶ�ָ��xС��λ��,Ĭ�ϵڶ�ָ��xλ��С
            GestureData.MaxPositionX = false;
            //�Ƿ�����Ѿ�ѡ���ͼ��
            if (GestureData.Fingers == 0)
            {
                foreach (GraphicsBase g in drawingCanvas.GraphicsList)
                {
                    g.IsOption = false;
                }
            }
        }
        /// <summary>
        /// Default cursor
        /// </summary>
        public static Cursor DefaultCursor
        {
            get
            {
                return Cursors.Arrow;
            }
        }

        /// <summary>
        /// Select all graphic objects
        /// </summary>
        public static void SelectAll(DrawingCanvas drawingCanvas)
        {
            for(int i = 0; i < drawingCanvas.Count; i++)
            {
                drawingCanvas[i].IsSelected = true;
            }
        }

        /// <summary>
        /// Unselect all graphic objects
        /// </summary>
        public static void UnselectAll(DrawingCanvas drawingCanvas)
        {
            for (int i = 0; i < drawingCanvas.Count; i++)
            {
                drawingCanvas[i].IsSelected = false;
            }
        }

        /// <summary>
        /// Delete selected graphic objects
        /// </summary>
        public static void DeleteSelection(DrawingCanvas drawingCanvas)
        {
            CommandDelete command = new CommandDelete(drawingCanvas);
            bool wasChange = false;

            for (int i = drawingCanvas.Count - 1; i >= 0; i--)
            {
                if ( drawingCanvas[i].IsSelected )
                {
                    drawingCanvas.GraphicsList.RemoveAt(i);
                    wasChange = true;
                }
            }

            if ( wasChange )
            {
                drawingCanvas.AddCommandToHistory(command);
            }
        }

        /// <summary>
        /// Delete all graphic objects
        /// </summary>
        public static void DeleteAll(DrawingCanvas drawingCanvas)
        {
            if (drawingCanvas.GraphicsList.Count > 0 )
            {
                drawingCanvas.AddCommandToHistory(new CommandDeleteAll(drawingCanvas));

                drawingCanvas.GraphicsList.Clear();
            }

        }
        
        /// <summary>
        /// Move selection to front
        /// </summary>
        public static void MoveSelectionToFront(DrawingCanvas drawingCanvas)
        {
            // Moving to front of z-order means moving
            // to the end of VisualCollection.

            // Read GraphicsList in the reverse order, and move every selected object
            // to temporary list.

            List<GraphicsBase> list = new List<GraphicsBase>();

            CommandChangeOrder command = new CommandChangeOrder(drawingCanvas);

            for(int i = drawingCanvas.Count - 1; i >= 0; i--)
            {
                if ( drawingCanvas[i].IsSelected )
                {
                    list.Insert(0, drawingCanvas[i]);
                    drawingCanvas.GraphicsList.RemoveAt(i);
                }
            }

            // Add all items from temporary list to the end of GraphicsList
            foreach(GraphicsBase g in list)
            {
                drawingCanvas.GraphicsList.Add(g);
            }

            if ( list.Count > 0 )
            {
                command.NewState(drawingCanvas);
                drawingCanvas.AddCommandToHistory(command);
            }
        }

        /// <summary>
        /// Move selection to back
        /// </summary>
        public static void MoveSelectionToBack(DrawingCanvas drawingCanvas)
        {
            // Moving to back of z-order means moving
            // to the beginning of VisualCollection.

            // Read GraphicsList in the reverse order, and move every selected object
            // to temporary list.

            List<GraphicsBase> list = new List<GraphicsBase>();

            CommandChangeOrder command = new CommandChangeOrder(drawingCanvas);

            for (int i = drawingCanvas.Count - 1; i >= 0; i--)
            {
                if (drawingCanvas[i].IsSelected)
                {
                    list.Add(drawingCanvas[i]);
                    drawingCanvas.GraphicsList.RemoveAt(i);
                }
            }

            // Add all items from temporary list to the beginning of GraphicsList
            foreach (GraphicsBase g in list)
            {
                drawingCanvas.GraphicsList.Insert(0, g);
            }

            if (list.Count > 0)
            {
                command.NewState(drawingCanvas);
                drawingCanvas.AddCommandToHistory(command);
            }
        }

        /// <summary>
        /// Apply new line width
        /// </summary>
        public static bool ApplyLineWidth(DrawingCanvas drawingCanvas, double value, bool addToHistory)
        {
            CommandChangeState command = new CommandChangeState(drawingCanvas);
            bool wasChange = false;


            // LineWidth is set for all objects except of GraphicsText.
            // Though GraphicsText has this property, it should remain constant.

            foreach(GraphicsBase g in drawingCanvas.Selection)
            {
                if (g is GraphicsRectangle ||
                     g is GraphicsEllipse ||
                     g is GraphicsLine ||
                     g is GraphicsPolyLine)
                {
                    if ( g.LineWidth != value )
                    {
                        g.LineWidth = value;
                        wasChange = true;
                    }
                }
            }

            if (wasChange  && addToHistory)
            {
                command.NewState(drawingCanvas);
                drawingCanvas.AddCommandToHistory(command);
            }

            return wasChange;
        }

        /// <summary>
        /// Apply new color
        /// </summary>
        public static bool ApplyColor(DrawingCanvas drawingCanvas, Color value, bool addToHistory)
        {
            CommandChangeState command = new CommandChangeState(drawingCanvas);
            bool wasChange = false;

            foreach (GraphicsBase g in drawingCanvas.Selection)
            {
                if (g.ObjectColor != value)
                {
                    g.ObjectColor = value;
                    wasChange = true;
                }
            }

            if ( wasChange && addToHistory )
            {
                command.NewState(drawingCanvas);
                drawingCanvas.AddCommandToHistory(command);
            }

            return wasChange;
        }

        /// <summary>
        /// Apply new font family
        /// </summary>
        public static bool ApplyFontFamily(DrawingCanvas drawingCanvas, string value, bool addToHistory)
        {
            CommandChangeState command = new CommandChangeState(drawingCanvas);
            bool wasChange = false;

            foreach (GraphicsBase g in drawingCanvas.Selection)
            {
                GraphicsText gt = g as GraphicsText;

                if (gt != null)
                {
                    if (gt.TextFontFamilyName != value)
                    {
                        gt.TextFontFamilyName = value;
                        wasChange = true;
                    }
                }
            }

            if (wasChange  && addToHistory )
            {
                command.NewState(drawingCanvas);
                drawingCanvas.AddCommandToHistory(command);
            }

            return wasChange;
        }

        /// <summary>
        /// Apply new font style
        /// </summary>
        public static bool ApplyFontStyle(DrawingCanvas drawingCanvas, FontStyle value, bool addToHistory)
        {
            CommandChangeState command = new CommandChangeState(drawingCanvas);
            bool wasChange = false;

            foreach (GraphicsBase g in drawingCanvas.GraphicsList)
            {
                if (g.IsSelected)
                {
                    GraphicsText gt = g as GraphicsText;

                    if (gt != null)
                    {
                        if (gt.TextFontStyle != value)
                        {
                            gt.TextFontStyle = value;
                            wasChange = true;
                        }
                    }
                }
            }

            if (wasChange  && addToHistory)
            {
                command.NewState(drawingCanvas);
                drawingCanvas.AddCommandToHistory(command);
            }

            return wasChange;
        }

        /// <summary>
        /// Apply new font weight
        /// </summary>
        public static bool ApplyFontWeight(DrawingCanvas drawingCanvas, FontWeight value, bool addToHistory)
        {
            CommandChangeState command = new CommandChangeState(drawingCanvas);
            bool wasChange = false;

            foreach (GraphicsBase g in drawingCanvas.Selection)
            {
                GraphicsText gt = g as GraphicsText;

                if (gt != null)
                {
                    if (gt.TextFontWeight != value)
                    {
                        gt.TextFontWeight = value;
                        wasChange = true;
                    }
                }
            }

            if (wasChange  && addToHistory)
            {
                command.NewState(drawingCanvas);
                drawingCanvas.AddCommandToHistory(command);
            }

            return wasChange;
        }

        /// <summary>
        /// Apply new font stretch
        /// </summary>
        public static bool ApplyFontStretch(DrawingCanvas drawingCanvas, FontStretch value, bool addToHistory)
        {
            CommandChangeState command = new CommandChangeState(drawingCanvas);
            bool wasChange = false;

            foreach (GraphicsBase g in drawingCanvas.Selection)
            {
                GraphicsText gt = g as GraphicsText;

                if (gt != null)
                {
                    if (gt.TextFontStretch != value)
                    {
                        gt.TextFontStretch = value;
                        wasChange = true;
                    }
                }
            }

            if (wasChange  && addToHistory)
            {
                command.NewState(drawingCanvas);
                drawingCanvas.AddCommandToHistory(command);
            }

            return wasChange;
        }

        /// <summary>
        /// Apply new font size
        /// </summary>
        public static bool ApplyFontSize(DrawingCanvas drawingCanvas, double value, bool addToHistory)
        {
            CommandChangeState command = new CommandChangeState(drawingCanvas);
            bool wasChange = false;

            foreach (GraphicsBase g in drawingCanvas.Selection)
            {
                GraphicsText gt = g as GraphicsText;

                if (gt != null)
                {
                    if (gt.TextFontSize != value)
                    {
                        gt.TextFontSize = value;
                        wasChange = true;
                    }
                }
            }

            if (wasChange && addToHistory)
            {
                command.NewState(drawingCanvas);
                drawingCanvas.AddCommandToHistory(command);
            }

            return wasChange;
        }


        /// <summary>
        /// Dump graphics list (for debugging)
        /// </summary>
        [Conditional("DEBUG")]
        public static void Dump(VisualCollection graphicsList, string header)
        {
            Trace.WriteLine("");
            Trace.WriteLine(header);
            Trace.WriteLine("");

            foreach(GraphicsBase g in graphicsList)
            {
                g.Dump();
            }
        }

        /// <summary>
        /// Dump graphics list overload
        /// </summary>
        [Conditional("DEBUG")]
        public static void Dump(VisualCollection graphicsList)
        {
            Dump(graphicsList, "Graphics List");
        }

        /// <summary>
        /// Return true if currently active properties (line width, color etc.)
        /// can be applied to selected items.
        /// 
        /// If at least one selected object has property different from currently
        /// active property value, properties can be applied.
        /// </summary>
        public static bool CanApplyProperties(DrawingCanvas drawingCanvas)
        {
            foreach(GraphicsBase graphicsBase in drawingCanvas.GraphicsList)
            {
                if ( ! graphicsBase.IsSelected )
                {
                    continue;
                }

                // ObjectColor - used in all graphics objects
                if ( graphicsBase.ObjectColor != drawingCanvas.ObjectColor )
                {
                    return true;
                }

                GraphicsText graphicsText = graphicsBase as GraphicsText;

                if ( graphicsText == null )
                {
                    // LineWidth - used in all objects except of GraphicsText
                    if ( graphicsBase.LineWidth != drawingCanvas.LineWidth )
                    {
                        return true;
                    }
                }
                else
                {
                    // Font - for GraphicsText

                    if ( graphicsText.TextFontFamilyName != drawingCanvas.TextFontFamilyName )
                    {
                        return true;
                    }

                    if ( graphicsText.TextFontSize != drawingCanvas.TextFontSize )
                    {
                        return true;
                    }

                    if ( graphicsText.TextFontStretch != drawingCanvas.TextFontStretch )
                    {
                        return true;
                    }

                    if ( graphicsText.TextFontStyle != drawingCanvas.TextFontStyle )
                    {
                        return true;
                    }

                    if ( graphicsText.TextFontWeight != drawingCanvas.TextFontWeight )
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Apply currently active properties to selected objects
        /// </summary>
        public static void ApplyProperties(DrawingCanvas drawingCanvas)
        {
            // Apply every property.
            // Call every Apply* function with addToHistory = false.
            // History is updated here and not in called functions.

            CommandChangeState command = new CommandChangeState(drawingCanvas);
            bool wasChange = false;

            // Line Width
            if ( ApplyLineWidth(drawingCanvas, drawingCanvas.LineWidth, false))
            {
                wasChange = true;
            }

            // Color
            if ( ApplyColor(drawingCanvas, drawingCanvas.ObjectColor, false) )
            {
                wasChange = true;
            }

            // Font properties
            if ( ApplyFontFamily(drawingCanvas, drawingCanvas.TextFontFamilyName, false) )
            {
                wasChange = true;
            }

            if ( ApplyFontSize(drawingCanvas, drawingCanvas.TextFontSize, false) )
            {
                wasChange = true;
            }

            if ( ApplyFontStretch(drawingCanvas, drawingCanvas.TextFontStretch, false) )
            {
                wasChange = true;
            }

            if ( ApplyFontStyle(drawingCanvas, drawingCanvas.TextFontStyle, false) )
            {
                wasChange = true;
            }

            if ( ApplyFontWeight(drawingCanvas, drawingCanvas.TextFontWeight, false) )
            {
                wasChange = true;
            }

            if ( wasChange )
            {
                command.NewState(drawingCanvas);
                drawingCanvas.AddCommandToHistory(command);
            }
        }

    }
}
