using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        public void Post([FromBody]test2.Models.Dog dog)
        {
            if(dog == null)
            {
                BadRequest("400");
            }
            string output = JsonConvert.SerializeObject(dog);
            System.IO.File.WriteAllText(@".\DogFiles\" + dog.BreedName + ".json",output);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] Dictionary<string,string> dogUpdate)
        {
            var specificDog = dogs.Where(dog => dog.BreedName == id).FirstOrDefault();
            if (specificDog == null)
            {
                NotFound("Resource not found on server.");
            }
            else
            {
                if(dogUpdate.ContainsKey("wikipediaUrl"))
                {
                    specificDog.WikipediaUrl = dogUpdate["wikipediaUrl"];
                }
                if (dogUpdate.ContainsKey("breedName"))
                {
                    specificDog.BreedName = dogUpdate["breedName"];
                }
                if (dogUpdate.ContainsKey("description"))
                {
                    specificDog.Description = dogUpdate["description"];
                }
                
                System.IO.File.WriteAllText(@".\DogFiles\" + specificDog.BreedName + ".json", JsonConvert.SerializeObject(specificDog));
                if (specificDog.BreedName.ToLower()!=id.ToLower())
                {
                    System.IO.File.Delete(@".\DogFiles\" + id + ".json");
                }
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            if (dogs.Select(dog => dog.BreedName).Contains(id))
            {
                System.IO.File.Delete(@".\DogFiles\" + id + ".json");
            }
            else
            {
                    NotFound("Resource not found on server.");
            }
        }
    }
}
