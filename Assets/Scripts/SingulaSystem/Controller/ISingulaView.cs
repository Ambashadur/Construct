using UnityEngine;

namespace SingulaSystem.Controller {
    public interface ISingulaView {
        public void ShowSingulaJoin(int singulaId, Vector3 position);

        public void HideSingulaJoin(int singulaId);

        public void JoinSingulaModels(int masterSingulaId, int slaveSingulaId, Vector3 slaveSingulaPosition);
    }
}