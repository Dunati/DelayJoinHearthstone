
using System;
using System.Collections;
using UnityEngine;
namespace DelayJoinLib
{
    public static class DelayJoin
    {
        public static void FindGame(GameMgr gameMgr, PegasusShared.GameType type, int missionId, long deckId, long aiDeckId)
        {
            gameMgr.GetTransitionPopup().StartCoroutine(DelayedFindGame(gameMgr, type, missionId, deckId, aiDeckId));
        }

        public static IEnumerator DelayedFindGame(GameMgr gameMgr, PegasusShared.GameType type, int missionId, long deckId, long aiDeckId)
        {
            bool cancelling = false;
            gameMgr.GetTransitionPopup().m_cancelButton.AddEventListener(UIEventType.RELEASE, new UIEvent.Handler(x => { cancelling = true; }));
            float seconds = 5.0f + UnityEngine.Random.value * 55.0f;
            Debug.Log(string.Format("DelayedFindGame waiting {0:0.0} seconds", seconds));
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(seconds);
            do
            {
                yield return new WaitForEndOfFrame();
            }
            while (!Input.GetKey(KeyCode.BackQuote) && DateTime.Now < end && !cancelling);
            Debug.Log("DelayedFindGame finding now");
            Network.Get().FindGame(type, missionId, deckId, aiDeckId);
            yield return new WaitForSeconds(.2f);
            if(cancelling)
            {
                gameMgr.CancelFindGame();
            }
        }
    }
}
