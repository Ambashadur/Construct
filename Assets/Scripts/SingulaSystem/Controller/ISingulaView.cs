using UnityEngine;
using SingulaSystem.Model;

namespace SingulaSystem.Controller {
    public interface ISingulaView {
        public void ShowSingulaJoin(Singula singula, Vector3 position);

        public void HideSingulaJoin(int singulaId);

        public void JoinSingulaModels(int masterSingulaId, int slaveSingulaId, Vector3 slaveSingulaPosition);
    }
}