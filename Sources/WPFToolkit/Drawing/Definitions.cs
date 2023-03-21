using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace WPFToolkit.Drawing
{
    /// <summary>
    /// Defines drawing tool
    /// </summary>
    public enum ToolType
    {
        None,
        Pointer,
        Rectangle,
        Ellipse,
        Line,
        PolyLine,
        Eraser,
        Text,
        Max
    };
    public enum GestureId
    {
        None,
        ZOOM,
        PAN,
        RORATE,
        ERASE
    };
    public static class GestureData
    {
        //��ָ��������
        static int fingers = 0;
        //��һ����ָ��Id
        static int firstDeviceId = -1;
        //�ڶ�����ָ��Id
        static int secondDeviceId = -1;
        //��һ����ָ��������ʼ��
        static Point firstFingerBeginPoint = new Point(0,0);
        //��һ����ָ��ǰ��λ��
        static Point firstFingerCurPoint = firstFingerBeginPoint;
        //�Ƿ�������
        static bool isGesture = false;
        //Ĭ��Ϊ����¼�
        static bool isTouch = false;
        //�Ƿ����һ����ָ��ʼ��ֵ����Щ����ֻ��ʼ��һ�Σ�
        static bool isSignFoFingerOne = false;

        //�Ƿ���ڶ�����ָ��ʼ��ֵ����Щ����ֻ��ʼ��һ�Σ�
        static bool isSignForFingerTwo = false;

        /// <summary>
        /// �жϾ������Ƶ�һЩ������Ϣ
        /// </summary>
        //��ָ�ʼ�ľ���
        static int startDistInTwoFingers = 0;
        //�ڶ�����ָ�Ĵ�������ʼ��
        static Point secFingerBeginPoint = new Point(0,0);
        //�ڶ�����ָ�ĵ�ǰλ��
        static Point secFingerCurPoint = secFingerBeginPoint;
        //��ָ��ʼ������λ��
        static Point startCenterPointInTwoFingers = new Point(0, 0);
        //��һ����ָ֮ǰ�ĵ���Ϣ
        static Point preFirstFingerPoint;
        //�ڶ�����ָ֮ǰ�ĵ���Ϣ
        static Point preSecFingerPoint;
        //��ָ֮ǰ�ľ���
        static int preDistInTwoFingers = 0;
        
        //��preFirstFingerPoint��preSecFingerPoint��ֵ�ı�־��
        static bool isSign = true;
        //�Ƿ��ǷŴ�,����������Ϊtrue����������ƽ�Ʋ�̫�������ֿ�
        static bool isZoom = false;
        //��С�ߴ�
        static double zoomOutScale = 1.0;
        //�Ŵ�ߴ�
        static double zoomInScale = 1.0;
        //ʵ�ʳߴ�
        static double zoomScale = 1.0;
        //ƽ��x����
        static double panx = 0.0;
        //ƽ��y����
        static double pany = 0.0;
        //��ת�Ƕ�
        static double rotateAngle = 0;
        //���Ʒ�����������ӿ��Ʒ����ٶ�
        static int zoomCnt = 0;
        //������ת��������ӿ��Ʒ����ٶ�
        static int rorateCnt = 0;

        //�жϵ�ǰ��һָ��xС��λ�ã����ǵڶ�ָ��xС��λ��,Ĭ�ϵڶ�ָ��xλ��С
        static bool maxPositionX = false;

        public static int Fingers { get => fingers; set => fingers = value; }
        public static Point FirstFingerBeginPoint { get => firstFingerBeginPoint; set => firstFingerBeginPoint = value; }
        public static Point FirstFingerCurPoint { get => firstFingerCurPoint; set => firstFingerCurPoint = value; }
        public static bool IsGesture { get => isGesture; set => isGesture = value; }
        public static bool IsTouch { get => isTouch; set => isTouch = value; }
        public static int FirstDeviceId { get => firstDeviceId; set => firstDeviceId = value; }
        public static int SecondDeviceId { get => secondDeviceId; set => secondDeviceId = value; }
        public static int StartDistInTwoFingers { get => startDistInTwoFingers; set => startDistInTwoFingers = value; }
        public static Point SecFingerBeginPoint { get => secFingerBeginPoint; set => secFingerBeginPoint = value; }
        public static Point SecFingerCurPoint { get => secFingerCurPoint; set => secFingerCurPoint = value; }
        public static Point StartCenterPointInTwoFingers { get => startCenterPointInTwoFingers; set => startCenterPointInTwoFingers = value; }
        public static Point PreFirstFingerPoint { get => preFirstFingerPoint; set => preFirstFingerPoint = value; }
        public static Point PreSecFingerPoint { get => preSecFingerPoint; set => preSecFingerPoint = value; }
        public static bool IsSign { get => isSign; set => isSign = value; }
        public static int PreDistInTwoFingers { get => preDistInTwoFingers; set => preDistInTwoFingers = value; }
        public static bool IsZoom { get => isZoom; set => isZoom = value; }
        public static double ZoomOutScale { get => zoomOutScale; set => zoomOutScale = value; }
        public static double ZoomInScale { get => zoomInScale; set => zoomInScale = value; }
        public static double ZoomScale { get => zoomScale; set => zoomScale = value; }
        public static double Panx { get => panx; set => panx = value; }
        public static double Pany { get => pany; set => pany = value; }
        public static double RotateAngle { get => rotateAngle; set => rotateAngle = value; }
        public static int ZoomCnt { get => zoomCnt; set => zoomCnt = value; }
        public static int RorateCnt { get => rorateCnt; set => rorateCnt = value; }
        public static bool IsSignFoFingerOne { get => isSignFoFingerOne; set => isSignFoFingerOne = value; }
        public static bool IsSignForFingerTwo { get => isSignForFingerTwo; set => isSignForFingerTwo = value; }
        public static bool MaxPositionX { get => maxPositionX; set => maxPositionX = value; }
    }; 
    public static class Threshold
    {
        const int curToBeginMaxDistInFirstFinger = 500;
        const int firstFingerToSecFingerMaxDist = 75000;
        const int panMaxDist = 5000;
        const int scaleThreshold = 2;
        const double esp = 1E-5;
        const double inf = 1E+14;
        public static int CurToBeginMaxDistInFirstFinger => curToBeginMaxDistInFirstFinger;

        public static int FirstFingerToSecFingerMaxDist => firstFingerToSecFingerMaxDist;

        public static int ScaleThreshold => scaleThreshold;

        public static double Esp => esp;

        public static double Inf => inf;

        public static int PanMaxDist => panMaxDist;
    }
    /// <summary>
    /// Context menu command types
    /// </summary>
    internal enum ContextMenuCommand
    {
        SelectAll,
        UnselectAll,
        Delete, 
        DeleteAll,
        MoveToFront,
        MoveToBack,
        Undo,
        Redo,
        SerProperties
    };
}
