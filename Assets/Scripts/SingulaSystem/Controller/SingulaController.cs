using UnityEngine;
using SingulaSystem.Model;

namespace SingulaSystem.Controller {
    public class SingulaController {
        public Conventus Conventus;

        private int? _slaveSingulaId;
        private int? _masterSingulaId;
        private int? _masterSingulaPimpleId;

        private IDbController _dbController;
        private ISingulaView _singulaView;

        public void SingulaNear(int slaveSingulaId, int masterSingulaId, int masterSingulaPimpleId) {
            if (slaveSingulaId < 0 
                || slaveSingulaId >= Conventus.Singulas.Length
                || masterSingulaId < 0
                || masterSingulaId >= Conventus.Singulas.Length) return;

            var masterSingula = Conventus.Singulas[masterSingulaId];

            if (masterSingulaPimpleId < 0 || masterSingulaPimpleId >= masterSingula.Pimples.Length) return;

            var slaveSingula = Conventus.Singulas[slaveSingulaId];

            //TODO: Расчёт местоположения модели для демонстрации возможно положния модели
        }

        public void SingulaAway() {
            //TODO: Убрать модель демонстрирующую возможное положение модели
        }

        public void TryJoinSingula() {
             
        }

        public void TryDetachSingula(int singulaId) {

        }

        public bool CanTakeSingula(int singulaId) {
            return true;
        }

        public void DownloadConventus(int conventusId) {

        }
    }
}