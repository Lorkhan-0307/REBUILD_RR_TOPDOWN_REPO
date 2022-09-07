## Unity
1. Camera


2. Git 사용법. (git-flow 공부)
    -> Open SourceTree -> 화면 상단에 메뉴 -> Commit (커밋) 
                                            -> Library 폴더가 포함되어 있는지 ?
                                            -> Temp 폴더가 포함되어 있는지 ?
                                        -> Commit (커밋) 완료
                                            -> 내가 작성한 파일들을 서버에 올릴 준비가 완료 됨. (서버에 올릴 내용을 로컬에 준비 시켜 놓음)
                                            -> 커밋을 푸시하기전에 여러번 할 수 있다.
                                        -> Push
                                            -> 준비된 파일들을 서버에 다 업로드 한다.
                                            -> 내가 작업한 내용이 서버에 올라갔기 때문에, 함께 작업하는 작업자들이 내가 작성한 내용을 볼수 있는 단계.
                                        -> Push, 푸시를 하려고 하는데 푸시가 안되네?
                                            -> 왜?
                                                -> A 작업자가 Character.cs 파일을 수정했습니다.
                                                -> B 작업자가 무언가 작업한 내용을 서버에 먼저 푸시를 했습니다.
                                                -> A 작업자가 푸시를 하려고 했더니 B작업자가 푸시한 내용이 서버에 있어서 푸시 할 수가 없네요.
                                                    -> 
                                            -> 서버에 다른 작업자가 먼저 푸시한 내용이 존재 할때는, 
                                                    -> 먼저 작업한 작업자의 파일들을 Pull을 받고 -> Merge 후에 -> Push를 해야합나다.
                                                    -> Auto Merge
                                                        -> Conflict 난 파일이 없을때.
                                                        -> 사람이 해줄게 없다.
                                                        -> 보통은 이럻게 Auto Merge가 된다.
                                                    -> Merge
                                                        -> Conflict 났다.
                                                        -> A작업자와 B작업자가 같은 파일을 수정해 버렸네?
                                                        -> 한줄 한줄씩,,, 한땀한땀... A작업자 한줄을 선택할건지, B작업자 한줄을 선택 할 건지.. 
                                                            -> 이 한줄 한줄 선택 작업이 완료가 되면, 머지가 완료됩니다.
                                        -> Pull ()
                                            -> ??? 이건 뭘까.
                                            -> 서버에 업로드 되어있는 최신 코드나 작업 내용을 내 컴퓨터에 받는 과정.
                                                -> 내가 내 컴퓨터에서 작업을 시작하기전에 Pull을 받고 시작한다.
                                                    -> 1. Conflict를 최소화 하기 위해서, 
                                                    -> 2. 최신코드 상태에서 작업을 하는게 가장 좋겠죠 ?
                                        -> Pull Request (?)
                                            -> A작업자가 작업을 했어요, A작업자가 작업한 내용이.. 프로덕트에 포함되도 되는지 안심이 안돼.. 불안해... 누가 같이 확이해줬으면 좋겠어..
                                                -> A 작업자는, Pull Request 를 B작업자에게 요청 할 수 있어요.
                                                -> A 작업자 새 Branch "Character"를 하나 만듭니다.
                                                -> A 작업자가 열심히 Character Branch에서 ....... 많이 했습니다.
                                                -> A작업자가 작업이 끝나면, Main 브렌치에 내가 작업한 Character 브렌치 내용을 포함 시키고 싶다.
                                                    - Main: 프로덕트 브렌치.
                                                    - 근데, 아무리 생각해도 불안하고, 누가 내가 작업한 내용을 같이 봐줬으면 좋겠어...
                                                    - Chracter 브렌치에서 작업을 쭉 했을거잖아요? (Commit -> Push.. Commit -> Push.. 반복)
                                                    - B 작업자에게, "져기요 Character 브렌치 내용좀 확인해서 풀좀 받아주세요." 말로 하는게 아니라... 
                                                        -> Pull Request 라는게 있어요..!
                                                        -> 
                                            -> Pull Request, 상대방 입장에서 생각해보면, "내가 Push를 했으니 Pull을 받아줘."
                                                                                -> 풀을 받는 사람은 내가 푸시한 내용을 취소 할 수 있는 권한이 있다.
                                                                                -> 풀을 받아줄 사람이 내가 보낸 Pull Request를 취소 혹은 안받아주면 메인 브렌치에 내 코드는 죽어도 포함되지 않는다....
                                        -> .....
                                        -> ...
                                            -> 권장하는게.. 하루에 코드 쪼금이라도 짜고... 커밋하나라도 올리세요..
                                            -> 



3. Coordinate
    1. Model
    2. World
    3. Camera (=Viewport)
        - Otho
        - Perspective
    4. Canvas 
    5. Screen

4. Popup 
    - Example
        -> 화면 UI
            -> Popup1 열기
                -> Popup2 열기
                -> Popup2 닫기
            -> Popup3 열기
                -> Popup4 열기
                -> Popup4 닫기
            -> 
        PopupManager (Singleton) (class)
            
            // 특업 팝업을 오픈한다.
            // Instantiate Prefab
            // Popup.Open()

            public static void Open(popupId: string)
            {
                string prefabName = "Popup " + popupId;
                GameObject prefab = Resources.Load<GameObject>(prefabName);

                var popup = GameObject.Instantiate(prefab) as GameObject;
                popup.GetComponent<Popup>.Open();
            }                                                

            public static void Close(); // 화면에서 제일 위에 있는 팝업을 닫는다.
                                        // Popup.Close()

            // static void Close(popupId); // 특정 팝업을 닫는다.

            -> Stack<Popup> -> Push()
            -> List<Popup>  -> Pop()
                            -> Peek()
                            -> GetCount()
                            -> IsEmpty()
        Popup (abstract class)
            -> abstract void Open()
            -> abstract void Close()
        
        PopupSettings : Popup
        {
            override void Open()
            {

            }

            override void Close()
            {

            }
        }

    1. 팝업이 열려야 한다.
        - 화면에 클릭 가능한 UI를 업데이트 한다.
    2. 팝업이 클로즈 된다.
        - 화면에 클릭 가능한 UI를 업데이트 한다.

    3. 팝업이 여러개가 떴을때 제일 마지막에 열린 팝업이 제일 위로 보여져야 한다.
        3-1. 3번째 팝업 방금 열렸을때  30
        3-2. 3번째 팝업이 방금 닫혔다. 20
        3-3. 다시 3번째 팝업이 열렸다. 30

    4. 팝업이 뜨면 백그라운드는 블러가 들어가면 좋겠다. (optional) 
        - 가우시안 블러, ...스터디

    5. 팝업이 뜨면 팝업 뒤에 있는 UI들은 눌리면 안된다.
        - 투명이미지를 팝업에 백그라운드 이미지로 엄청 크게 넣는다. 
            (-> 팝업 뒤에는 안눌린다)
        - 