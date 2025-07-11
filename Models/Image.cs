using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models;

    public class Image
    {
        public long Id { get; set; }
        
        private string fileName = string.Empty;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        private byte[] imageData;
        public byte[] ImageBlob
        {
            get { return imageData; }
            set { imageData = value; }
        }
        private long fileSize = 0;

        private long ImageGroupId = 0;
        
        public long FileSize
        {
            get { return fileSize; }
            set {fileSize = value; }
        }
        public Image()
        { }
        
        public Image(string a, byte[] b, long c)
        {
            this.FileName = a;
            this.ImageBlob = b;
            this.FileSize = c;
        }
    }

