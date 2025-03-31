# 빙구레이스
---
### 개요

- 일시 : 24.11 ~ 24.12
- 목적 :
    - 방송용 랜덤적 요소를 조금 더 재밌게 시청자들에게 제공하기 위한 컨텐츠 제작을 위한 목적으로 개발하게 되었습니다

### 배울 수 있었던 점 요약

- 데이터 통신 시 두 시스템의 속도 차이로 인해 데이터 누락이 발생할 수 있기 때문에 속도가 비교적 느린 시스템에서 데이터를 누적 시킨 후 일괄처리를 해야한다는 것을 배웠습니다
---

### 사용된 WebSocket 서버

- https://github.com/Sonwon112/NicknameGameServer.git (공식 API 서버로 수정 예정)
---

### 사용도구 및 기술

- Unity
- WebSocket

---
### 제작 과정 및 핵심 기술

- 플레이되는 맵의 형태와 맵마다 발생되는 이벤트의 종류가 다양하게 필요했지만 개발 초기에 어떤 이벤트를 구현할지 확정되지 않았기 때문에  이벤트를 개발하면서 추가할때 이벤트를 호출하는 쪽에서 문제 없이 동작하게 하기 위해서 Event를 추상 클래스로 생성하여서 하위 클래스들로 이벤트 상세 내용을 작성하고 Event/를 추첨하는 EventCard 클래스에서 Event를 통해 호출하게 함으로써 확장성을  높혔습니다.

```csharp
// Event 추상 클래스
public abstract class Event : MonoBehaviour
{
    protected CharacterMovement target;

    public Sprite thumbnail;
    public string eventName;
    public int targetcnt = 1;
    public abstract void playEvent(CharacterMovement target);
    public Sprite getThumbnail() { return thumbnail; }
    public string getEventName() {
        string text = eventName.Replace("\\n", "\n");
        return text; 
    } 
}
//----------------------Event를 상속 중인 여러 종류의 이벤트--------------------
public class ForkEvent : Event ...
public class icyRoadEvent : Event ...
public class PresentEvent : Event ...
...

//--------------------------EventCard 추첨후 지정 부분-------------------------
								// 발생한 이벤트 오브젝트의 Event 컴포넌트로 얻어옴
								currEvent = eventsList[eventIndex].GetComponent<Event>();
                m_Thumbnail.sprite = currEvent.getThumbnail();
                m_Text.text = currEvent.getEventName();
....
// 이벤트를 실행 하는 부분
public void Resume()
    {
        Time.timeScale = 1;
        currEvent.playEvent(currTarget);
    }

```

- 현재 프로젝트에서는 Scene을 3개로 구성하였습니다(시작화면 Scene, 플레이할 맵을 고르는 목록 Scene, 실제로 게임이 구동되는 Scene) 플레이할 맵을 고르고 시청자들이 ‘!참여’를 입력함으로써 현재 플레이할 게임의 참여를 할 수 있어야 했습니다. 
 그렇다고 게임을 채팅창 WebSocket과 다이렉트로 연결하기엔 방송인이 관리하기엔 어려울 거 같다고 판단하여서 별도로 채팅 수집용 Spring Boot로 서버를 구축하였습니다.(평소 많이 사용했었고, WebSocket을 학습해보고 싶어서 선정하였습니다.)
 이제 Spring Boot 서버에 참여를 허용할 때 참여자에 대한 리스트를 얻어와야했기 때문에 Session을 유지하면서 통신하는 WebSocket 통신 방식을 선정하였습니다. Session을 유지하기 위해서 Singleton 패턴을 사용하였고, Scene 전환 시 삭제되지 않게하여 Session을 유지하였습니다

```csharp
public static GameManager gameManagerInstance { get; set; }
private static string TOKEN = "0niyaNicknameGame";
private void Awake()
{
    if(gameManagerInstance == null)
    {
        gameManagerInstance = this;
        DontDestroyOnLoad(gameObject);
    }else if(gameManagerInstance != this)
    {
        Destroy(gameObject);
    }
}
```

### 진행하면서 어려웠던 점 혹은 문제점

- 짧은 시간을 간격으로 들어오는 요청에 의해 일부 시청자가 누락되는 현상
    
     기존 제작 방식에서는 단순히 WebSocket 서버에서 닉네임 데이터를 보내면 String에 저장하고 Update가 되었을 때 화면을 갱신 시키는 방식을 사용하였습니다
    
    ```cpp
     		private string appendNickname;
     		
     		...
     		
     		private void Update()
     		{
    		 	if(callAppend){
    					 	participantWindow.AppendParticipant(appednNickname);
                callAppend = false;
    		 	}	
    	 		
     		}
     		
     		...
     		
     		public void gettingMessage(string msg)
        {
            appednNickname = msg;
            callAppend = true;
        }
    ```
    
     이 방식에서 unity에서의 Update가 갱신되는  속도보다 닉네임 데이터가 더 빠르게 들어와 입력된 값들이 누락되는 현상이 발생하였습니다. 그래서 아래와 같이 List에 누적 시킨 후 Update()함수에서 호출 시 누적된 닉네임을 한번에 갱신 시키는 방식으로 수정하였습니다.
    
    ```cpp
    	  private List<string> appendNickname = new List<string>();
    	  
    	  ...
    	  
    	  private void Update()
     		{
    		 	if(callAppend){
    					 	foreach(string nickName in appednNickname)
                {
                    participantWindow.AppendParticipant(nickName);
                }
                callAppend = false;
                appednNickname.Clear();
    		 	}	
    	 		
     		}
     		
     		...
     		
     		public void gettingMessage(string msg)
        {
            appednNickname.Add(msg);
            callAppend = true;
        }
    ```
    
     데이터 통신에서 두 시스템의 속도 차이도 고려해야 된다는 것을 배웠습니다.
