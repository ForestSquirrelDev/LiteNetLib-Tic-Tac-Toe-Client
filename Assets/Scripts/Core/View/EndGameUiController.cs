using System.Threading.Tasks;
using Core.Components;
using Core.Entities;
using Core.Managers;
using DG.Tweening;
using Server.Shared.Network;
using ServerShared.Shared.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class EndGameUiController : MonoBehaviour, INetMessageListener {
    [SerializeField] private GameModel _model;
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private TMP_Text _winText;

    [SerializeField] private VideoClip _winConditionVideo;
    [SerializeField] private VideoClip _loseConditionVideo;
    [SerializeField] private VideoClip _drawConditionVideo;
    [SerializeField] private RawImage _videoTexture;

    private void Start() {
        _model.IncomingMessagesPipe.Register(MessageType.GameOverMessage, this);
    }

    public async void ReceiveMessage(MessageWrapper messageWrapper) {
        if (messageWrapper.Message is not GameOverMessage gameOverMessage)
            return;
        
        var winningSide = (GameSide)gameOverMessage.Winner;
        var playerSide = _model.World.Entities.GetFirst<Player>().GetComponent<GameSideComponent>().GameSide;
        Debug.Log($"Winning side is {winningSide}. Player side is {playerSide}");
        
        var videoClip = GetAppropriateVideo(winningSide, playerSide);
        await PrepareVideoPlayer(_videoPlayer, videoClip);
        PrepareRectTransform(winningSide, playerSide, _videoTexture.rectTransform);
        var sequence = GetAppropriateSequence(winningSide, playerSide, _videoTexture.rectTransform, _videoPlayer);
        sequence.Play();
        
        _winText.gameObject.SetActive(true);
        _winText.text = GetAppropriateText(winningSide, playerSide);
    }

    private VideoClip GetAppropriateVideo(GameSide winningSide, GameSide playerSide) {
        return winningSide == GameSide.None ? _drawConditionVideo : playerSide == winningSide ? _winConditionVideo : _loseConditionVideo;
    }

    private async Task PrepareVideoPlayer(VideoPlayer player, VideoClip clip) {
        player.clip = clip;
        player.Play();
        player.SetDirectAudioMute(0, true);
        await Task.Delay(25);
        player.SetDirectAudioMute(0, false);
        player.Pause();
    }

    private Sequence GetAppropriateSequence(GameSide winningSide, GameSide playerSide, RectTransform textureRectTransform, VideoPlayer player) {
        var entranceSequence = DOTween.Sequence();
        const float startPosition = 2f;
        
        if (winningSide == GameSide.None) {
            entranceSequence.Insert(startPosition, textureRectTransform.DOAnchorPos3D(Vector3.zero, 1f));
            entranceSequence.InsertCallback(startPosition, player.Play);
            return entranceSequence;
        }
        if (winningSide == playerSide) {
            entranceSequence.Insert(startPosition, textureRectTransform.DORotate(new Vector3(0f, 0f, -360f * 4f), 2f, RotateMode.FastBeyond360).SetEase(Ease.Linear));
            entranceSequence.Insert(startPosition, textureRectTransform.DOAnchorPos3D(Vector3.zero, 1f));
            entranceSequence.InsertCallback(entranceSequence.Duration(), player.Play);
            entranceSequence.Play();
            return entranceSequence;
        }
        if (winningSide != playerSide) {
            entranceSequence.Insert(startPosition, textureRectTransform.DOScale(Vector3.one, 1.6f));
            entranceSequence.InsertCallback(startPosition, player.Play);
        }
        
        return entranceSequence;
    }

    private void PrepareRectTransform(GameSide winningSide, GameSide playerSide, RectTransform textureRectTransform) {
        if (winningSide == GameSide.None) {
            textureRectTransform.anchoredPosition = new Vector2(0f, -1600f);
            return;
        }
        if (winningSide == playerSide) {
            textureRectTransform.anchoredPosition = new Vector2(-1200f, 1000f);
            return;
        }
        if (winningSide != playerSide) {
            textureRectTransform.anchoredPosition = Vector2.zero;
            textureRectTransform.localScale = Vector3.zero;
        }
    }

    private string GetAppropriateText(GameSide winningSide, GameSide playerSide) {
        if (winningSide == GameSide.None) {
            return "It's a draw. The Wok is confused";
        }
        if (winningSide == playerSide) {
            return "You win! John Xina congratulates you";
        }
        return "You lose. How pity";
    }
}
