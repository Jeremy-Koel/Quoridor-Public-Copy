using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostedGameLobbies : MonoBehaviour
{
    RectTransform hostedGameLobbiesRectTransform;
    Button hostGameButton;
    public Button hostedGamePrefab;
    public List<Button> hostedGames;

    private void Awake()
    {
        hostedGameLobbiesRectTransform = GameObject.Find("HostedGameLobbies").GetComponent<RectTransform>();
        hostGameButton = GameObject.Find("HostGameButton").GetComponent<Button>();
        hostGameButton.onClick.AddListener(onHostGameButtonClick);
        hostedGames = new List<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onHostGameButtonClick()
    {
        // for now fake adding a game
        AddHostedGame();
    }

    // Get all game data from GS and add game
    void AddHostedGame()
    {
        // Get text data
        // temp get fake data
        string hostName = "blablablabla";
        AddHostedGameToLobbies(hostName);
    }

    // Add game data to lobbies in UI
    void AddHostedGameToLobbies(string hostName)
    {
        // Create new (gamelobby) Button to add to children of HostedGameLobbies
        Button hostedGameLobby = Instantiate(hostedGamePrefab) as Button;
        
        // Get text component of button
        UnityEngine.UI.Text[] hostedGameLobbyText = hostedGameLobby.GetComponentsInChildren<Text>();
        Text playerText = hostedGameLobbyText[0];
        //Text messageText = hostedGameLobbyText[1];

        if (hostName.Length >= 10)
        {
            playerText.text = ("<b>" + hostName.Substring(0, 10) + ":</b>");
        }
        else
        {
            playerText.text = ("<b>" + hostName + ":</b>");
        }

        //playerText.text = ("<b>" + messageWho + ":</b>");
        //messageText.text = (messageMessage);

        hostedGameLobby.transform.SetParent(hostedGameLobbiesRectTransform);
        hostedGameLobby.transform.localScale = new Vector3(1, 1, 1);

        hostedGames.Add(hostedGameLobby);

        LayoutRebuilder.ForceRebuildLayoutImmediate(hostedGameLobbiesRectTransform);
    }
}
