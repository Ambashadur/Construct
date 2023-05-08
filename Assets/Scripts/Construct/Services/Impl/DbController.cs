using System.IO;
using UnityEngine;

namespace Construct.Services.Impl {
    public class DbController : IDbController {
        private readonly string _path = $"{Application.dataPath}/Scripts/Construct/Services/Impl/hirearchy.json";

        public ConventusDto DonwloadConventus(int id) {
            var fileContent = File.ReadAllText(_path);
            return JsonUtility.FromJson<ConventusDto>(fileContent);
        }
    }
}
