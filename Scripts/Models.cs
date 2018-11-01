using System;

namespace Paperol
{
    [Serializable]
    public struct TextLineData
    {
        public string textbody;
        public int size;
        public string highlightColor;
        public string color;
        public bool bold;
        public int id;
        public int posX;
        public int posY;
        public bool constrainX;
        public bool constrainY;
    }

    [Serializable]
    public struct ArrowData
    {
        public int posX;
        public int posY;
        public int toX;
        public int toY;
    }

    public struct LineData
    {
        public int posX;
        public int posY;
        public float direction;
    }

    [Serializable]
    public struct PaperolData
    {
        public string title;
        public int id;
        public string url;
        public string author;
        public bool constrainX;
        public bool constrainY;
        public float initialTransformX;
        public float initialTransformY;
        public float initialZoom;
        public int timeLineDirection;
        public float timeLineSpeed;
        public bool showTimeLines;
        public TextLineData[] textLines;
        public ArrowData[] arrows;
        public LineData[] lines;
    }

    public struct PaperolDataList
    {
        public PaperolData[] data;
    }

}
