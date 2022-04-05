using CrudClienteMongoDB.Config;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudClienteMongoDB.Models
{
    public class ClienteContexto
    {
        private IMongoDatabase _dbAccess;

        public ClienteContexto(IOptions<ConfigDb> options)
        {
            MongoClient mongoClient = new MongoClient(options.Value.ConnectionString);

            if (mongoClient != null)
            {
                _dbAccess = mongoClient.GetDatabase(options.Value.Database);
            }
        }

        public IMongoCollection<Clientes> Clientes  
        {
            get
            {
                return _dbAccess.GetCollection<Clientes>("Clientes");
            }
        } 
    }
}
