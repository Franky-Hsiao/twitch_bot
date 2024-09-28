# twitch_bot
## 簡介
  Twitch 做為世界上最多人使用的直播平台，每日都有上千萬的用戶在使用，然而在這個平台裡面，針對觀眾與實況主的互動功能卻有所欠缺，不然就是要使用付費功能才能達到一些效果，因此我設計了一個可以讓觀眾直接留言互動的機器人。
  
  這個用於Twitch直播中的聊天機器人，可以打字互動來改變直播畫面上的人物動作，或是在玩遊戲時可以做排隊、查看、取消等功能，利用了 twitchio 輸出入聊天室訊息和 Twitch API 處理用戶數據
  動畫和其他部分則是使用C#去做處理。目前只能針對特定字串做處理或互動，預計未來要融合AI語意判斷來讓體驗更方便。

  從開始使用以來，這個作品已在 100 多場直播中使用，累計處理了 8500 多則觀眾訊息，並有效的提高了觀眾在直播中的互動意願。
  
### 互動人物範例
![初始狀態](https://github.com/Franky-Hsiao/twitch_bot/blob/main/twitchBot/image/chibiwalk.gif)
![變化後狀態A](https://github.com/Franky-Hsiao/twitch_bot/blob/main/twitchBot/image/chibiConfuse.gif)
![變化後狀態B](https://github.com/Franky-Hsiao/twitch_bot/blob/main/twitchBot/image/chibiNoHeadwear.gif)

### 特色

- 及時處理聊天室請求
- 提供觀眾互動管道
- 紀錄相關文字及直播數據

### 相關技術

- 使用了python和TwitchIO做機器人和聊天室的訊息傳遞
- 利用Twitch API做相關的數據統計
- 用C#寫顯示在畫面上互動動畫的應用程式
- 用Socket在本地端傳遞機器人和動畫應用程式的訊息

## 相關資源
[twitchio](https://twitchio.dev/en/stable/)  
[Twitch API](https://dev.twitch.tv/docs/api/)
