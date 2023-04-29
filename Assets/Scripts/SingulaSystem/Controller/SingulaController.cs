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

        public SingulaController(ISingulaView singulaView) {
            _singulaView = singulaView;
        }

        public void SingulaNear(
            Singula slaveSingula, 
            Singula masterSingula, 
            int masterSingulaPimpleId)
        {
            if (!slaveSingula.Slot.HasValue
                || Conventus.Hirearchy[slaveSingula.Id] != masterSingula.Id) return;

            var slaveSingulaPosition = masterSingula.transform.TransformPoint(
                masterSingula.Pimples[masterSingulaPimpleId].Position - slaveSingula.Slot.Value);

            _singulaView.ShowSingulaJoin(slaveSingula, slaveSingulaPosition);
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