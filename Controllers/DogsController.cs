using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using test2.Models;

namespace dogapi_SofiaZaid.Controllers
{
    //man kan ta bort api härifrån, man behöver ej lägga till id i route här- om man matar in ett id i sin uri så
    //väljer den get metoden som tar ett id.
    [Route("[controller]")]
    public class DogsController : Controller
    {
        private List<Dog> dogs = new List<Dog>();
        public DogsController()
        {
            var files = System.IO.Directory.GetFiles("DogFiles", "*.json");
            foreach (var file in files)
            {
                dogs.Add(JsonConvert.DeserializeObject<Dog>(System.IO.File.ReadAllText(file)));
            }
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return dogs.Select(d => d.BreedName).ToArray();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            return dogs.Where(x => x.BreedName == id).Select(dog => JsonConvert.SerializeObject(dog)).FirstOrDefault();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
            //dogs.Add(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            //dogs[id] = value;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //dogs.RemoveAt(id);
        }
    }
}
