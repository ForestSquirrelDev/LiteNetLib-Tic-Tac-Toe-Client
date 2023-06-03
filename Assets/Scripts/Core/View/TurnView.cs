using Core.Managers;
using Server.Shared.Network;
using ServerShared.Shared.Network;
using UnityEngine;

public class TurnView : MonoBehaviour, INetMessageListener {
    [SerializeField] private GameModel _model;
    [SerializeField] private GameObject _turnText;
    [SerializeField] private GameObject _turnCross;
    [SerializeField] private GameObject _turnCircle;
    [SerializeField] private GameObject _turnQuestionMark;

    [SerializeField] private GameObject _sideText;
    [SerializeField] private GameObject _sideCross;
    [SerializeField] private GameObject _sideCircle;

    private void Start() {
        _model.IncomingMessagesPipe.Register(MessageType.GameStartedMessage, this);
        _model.IncomingMessagesPipe.Register(MessageType.AcceptJoinMessage, this);
        _model.IncomingMessagesPipe.Register(MessageType.TurnFinished, this);
        _model.IncomingMessagesPipe.Register(MessageType.GameOverMessage, this);
    }

    public void ReceiveMessage(MessageWrapper messageWrapper) {
        switch (messageWrapper.Message) {
            case AcceptJoinMessage acceptJoin:
                ProcessJoin(_sideText, (GameSide)acceptJoin.GameSide, _sideCross, _sideCircle);
                break;
            case GameStartedMessage gameStarted:
                ProcessGameStart(_turnQuestionMark, (GameSide)gameStarted.FirstTurnSide, _turnCross, _turnCircle);
                break;
            case TurnFinishedMessage turnFinishedMessage:
                ProcessTurnFinish(_turnText, _turnQuestionMark, (GameSide)turnFinishedMessage.GameSide, _turnCircle, _turnCross);
                break;
            case GameOverMessage _:
                ProcessGameOver(_turnText, _turnQuestionMark, _turnCircle, _turnCross, _sideText, _sideCross, _sideCircle);
                break;
        }
    }
    
    private void ProcessJoin(GameObject sideText, GameSide gameSide, GameObject sideCross, GameObject sideCircle) {
        sideText.SetActive(true);
        ShuffleGameobjectStates(gameSide == GameSide.Cross ? sideCross : sideCircle, gameSide == GameSide.Cross ? sideCircle : sideCross);
    }

    private void ProcessGameStart(GameObject turnQuestionMark, GameSide gameSide, GameObject turnCross, GameObject turnCircle) {
        turnQuestionMark.SetActive(false);
        var activeTurn = gameSide == GameSide.Cross ? turnCross : turnCircle;
        var inactiveTurn = gameSide == GameSide.Cross ? turnCircle : turnCross;
        ShuffleGameobjectStates(activeTurn, inactiveTurn);
    }

    private void ProcessTurnFinish(GameObject turnText, GameObject turnQuestionMark, GameSide gameSide, GameObject turnCircle, GameObject turnCross) {
        turnText.SetActive(true);
        turnQuestionMark.SetActive(false);
        var activeTurn = gameSide == GameSide.Cross ? turnCircle : turnCross;
        var inactiveTurn = gameSide == GameSide.Cross ? turnCross : turnCircle;
        ShuffleGameobjectStates(activeTurn, inactiveTurn);
    }

    private void ShuffleGameobjectStates(GameObject activeObject, GameObject inactiveObject) {
        activeObject.SetActive(true);
        inactiveObject.SetActive(false);
    }
    
    private void ProcessGameOver(params GameObject[] objectsToDisable) {
        foreach (var obj in objectsToDisable) {
            obj.SetActive(false);
        }
    }
}
