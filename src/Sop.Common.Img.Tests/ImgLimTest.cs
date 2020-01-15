using NUnit.Framework;
using Sop.Common.Img.Servers;
using System;

namespace Sop.Common.Img.Tests
{
    /// <summary>
    /// ��JPEG��PNG��ʽ��ͼƬʵʱѹ���������ܲ�Ӱ�컭��
    /// </summary>
    public class ImgLimTest
    {
        private string[] _imageFilePaths;
        private string _outputFilePath;
        private string _imagePath;
        private string _filePath;
        private IImgLim _imagesLim;

        [SetUp]
        public void Setup()
        {
            _imagesLim = new ImgLim();
            string path = AppDomain.CurrentDomain.BaseDirectory;
            _filePath = $"{path}Resources\\";

            //����ͼƬ����
            //http://7xkv1q.com1.z0.glb.clouddn.com/grape.jpg?imageslim

            //Ĭ���ļ��ڡ�Resources����imageslim���ļ��У����������ԣ�����������ƣ�

            var name = "grape-imageslim-" + Guid.NewGuid().ToString("N");
            _outputFilePath = $"{_filePath}output\\{name}.jpg";
        }
        [Test]
        public void GetThumbnails_Tests()
        {
            _imagePath = $"{_filePath}imageslim\\grape.jpg";
            var isok = _imagesLim.GetThumbnails(_imagePath, _outputFilePath);
            Assert.IsTrue(isok, "�ɹ�");
        }

        [Test]
        public void GetThumbnails_Tests_By_Png()
        {
            var isok = _imagesLim.GetThumbnails(_imagePath, _outputFilePath);
            Assert.IsTrue(isok, "�ɹ�");
        }

    }
}