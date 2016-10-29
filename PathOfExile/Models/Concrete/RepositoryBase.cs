using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using PathOfExile.Models.Services;

namespace PathOfExile.Models.Concrete
{
    public abstract class RepositoryBase<TEntity> where TEntity : class
    {
        internal string _path;
        internal IEnumerable<TEntity> _data;
        public IEnumerable<TEntity> Get()
        {
            if (!File.Exists(_path))
            {
                _data = new List<TEntity>();
                
            }
            else
                _data = XMLService<TEntity>.Deserialize(_path);
            return _data;

        }

        public void Save()
        {
            XMLService<TEntity>.Save(_data, _path);
        }

    }
}
