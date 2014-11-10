using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVRControlServer.DataBase.Model
{
    class ImageInfo
    {
        private Int64 AutoID;
        private bool Active;
        private bool Deleted;
        private DateTime CreateDate;
        private DateTime UpdateDate;
        private string CreateBy;
        private string UpdateBy;
        private string Path;
        private Int64 NodeID;
        private string Name;
        private DateTime ImageDate;
        private string Remark;


        public void setAutoID(Int64 autoId)
        {
            this.AutoID = autoId;
        }
        public Int64 getAutoID()
        {
            return AutoID;
        }
        public void setActive(bool active)
        {
            this.Active = active;
        }
        public bool getActive()
        {
            return Active;
        }
        public void setDeleted(bool deleted)
        {
            this.Deleted = deleted;
        }
        public bool getDeleted()
        {
            return Deleted;
        }

        public void setCreateDate(DateTime createdate)
        {
            this.CreateDate = createdate;
        }
        public DateTime getCreateDate()
        {
            return CreateDate;
        }
        public void setUpdateDate(DateTime updatedate)
        {
            this.UpdateDate = updatedate;
        }
        public DateTime getUpdateDate()
        {
            return UpdateDate;
        }
        public void setCreateBy(string createby)
        {
            this.CreateBy = createby;
        }
        public string getCreateBy()
        {
            return CreateBy;
        }
        public void setUpdateBy(string updateby)
        {
            this.UpdateBy = updateby;
        }
        public string getUpdateBy()
        {
            return UpdateBy;
        }
        public void setNodeId(Int64 nodeid)
        {
            this.NodeID = nodeid;
        }
        public Int64 getNodeId()
        {
            return NodeID;
        }
        public void setRemark(string remark)
        {
            this.Remark = remark;
        }
        public string getRemark()
        {
            return Remark;
        }
        public void setName(string name)
        {
            this.Name = name;
        }
        public string getName()
        {
            return Name;
        }
        public void setPath(string path)
        {
            this.Path = path;
        }
        public string getPath()
        {
            return Path;
        }
        public void setImageDate(DateTime imagedate)
        {
            this.ImageDate = imagedate;
        }
        public DateTime getImageDate()
        {
            return ImageDate;
        }
    }
}
