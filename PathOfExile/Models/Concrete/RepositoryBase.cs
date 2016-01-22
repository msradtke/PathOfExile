using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
namespace PathOfExile.Models.Concrete
{
    public class RepositoryBase<TEntity> where TEntity : class
    {
        private string _path;
        public RepositoryBase()
        {
            _path = System.Reflection.Assembly.GetExecutingAssembly().Location;
        }
 
        public IEnumerable<TEntity> Get()
        {
            XmlSerializer serializer =
            new XmlSerializer(typeof(TEntity));
            IEnumerable<TEntity> entities = new List<TEntity>();

            using (StreamReader file = new StreamReader(_path))
            {
                entities = (IEnumerable<TEntity>)serializer.Deserialize(file);
            }

            return entities;
        }


    }
}
