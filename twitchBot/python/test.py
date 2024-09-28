import string
from twitchio.ext import commands
import os
from dotenv import load_dotenv
from datetime import datetime
import json
import random
import asyncio 
import socket
from twitchio.ext import pubsub
import speech_recognition as sr
import sys
import threading
from twitchAPI.twitch import Twitch
print(sys.path)
load_dotenv()

nameList=[]
lucky = {}
message_log=[]


twitch = Twitch(os.environ['bot_id'], os.environ['bot_key']) 
class TwitchBot(commands.Bot):
    stop=False
    def __init__(self):
        super().__init__(token=os.environ['token'],
                         nick=os.environ['nick'],
                         prefix=os.environ['prefix'],
                         client_id=os.environ['client_id'],
                         initial_channels=[os.environ['initial_channels']])
       
    async def event_ready(self):
        await twitch.authenticate_app([])
        
    async def event_message(self, message):
        if hasattr(message.author, 'name') and message.author.name != "nightbot":
            
            message_data = {
                "author": message.author.name,
                "content": message.content,
                "timestamp": str(message.timestamp)
            }
            message_log.append(message_data)

            with open("..\\..\\python\\message_log.json", "a", encoding="utf-8") as log_file:
                log_file.write(json.dumps(message_data, ensure_ascii=False) + "\n")
            
            await twitchBot.handle_commands(message)
    '''
    @commands.command(name="註冊")
    async def register(self, ctx):
        user_info = twitch.get_users(user_ids=["693183068"])
        
        async for user in user_info:
            user_id = user.id
            display_name = user.display_name
            #follower_count = await self.get_follower_count(user_id)

            await ctx.send(f"用戶ID: {user_id}")
            await ctx.send(f"頻道名稱: {display_name}")
            #await ctx.send(f"追隨人數: {follower_count}")
    '''
    @commands.command(name="中止") 
    async def stopQueue(self,ctx): 
        if ctx.author.name == "hello_i_am_87":
            self.stop=True
            await ctx.send("排隊截止！")
        else:
            await ctx.send("不給你用~")
    @commands.command(name="測試") 
    async def test(self,ctx): 
        print("getMessage")
        await ctx.send("測試成功！")
    @commands.command(name="排隊") 
    async def queue(self,ctx):
        if self.stop == True:
            await ctx.send("今天排隊截止了！")
        elif ctx.author.display_name in nameList:
            await ctx.send("你排隊過了喔！")
        else:
            tmp=len(nameList)
            if tmp == 0:  
                await ctx.send("排隊成功，馬上換你喔！")
            else :
                await ctx.send("排隊成功，你前面還有"+str(tmp)+"個人！")
            nameList.append(str(ctx.author.display_name))

    @commands.command(name="目前") 
    async def now(self,ctx):
        strNameList = "目前排隊有："
        if len(nameList) != 0:
            for s in nameList:
                strNameList = strNameList + s + " || " 
            strNameList = strNameList[:-4]
            await ctx.send(strNameList)
        else:
            await ctx.send("還沒有人排隊...")

    @commands.command(name="戳") 
    async def stamp(self,ctx): 
        print("睡覺")
        message = "confuse"
        full_message = f"{message}\n" 
        csharp_socket.send(full_message.encode())
        await ctx.send("？")
    @commands.command(name="走路") 
    async def walk(self,ctx): 
        print("走路")
        message = "walk"
        full_message = f"{message}\n"
        csharp_socket.send(full_message.encode()) 
    @commands.command(name="抓") 
    async def grap(self,ctx):
        print("抓")
        message = "headwear"
        full_message = f"{message}\n"  
        csharp_socket.send(full_message.encode())
        #await ctx.send("耳朵QQ")
    @commands.command(name="排队") 
    async def queue_2(self,ctx):
        if self.stop == True:
            await ctx.send("今天排隊截止了！")
        elif ctx.author.display_name in nameList:
            await ctx.send("你排隊過了喔！")
        else:
            tmp=len(nameList)
            if tmp == 0:  
                await ctx.send("排隊成功，馬上換你喔！")
            else :
                await ctx.send("排隊成功，你前面還有"+str(tmp)+"個人！")
            nameList.append(str(ctx.author.display_name))
       
        
    @commands.command(name="插隊") 
    async def insert_queue(self,ctx):
         await ctx.send("不行！！")

    @commands.command(name="刪除")
    async def delete(self,ctx, num_to_delete: int = 1):  # 接受一個整數參數，預設為1
        if ctx.author.name == "hello_i_am_87":
            if num_to_delete <= len(nameList):
                for i in range(num_to_delete):
                    del nameList[0]
                await ctx.send("成功刪除！")
            else:
                await ctx.send("刪除太多了!！")
        else:
            await ctx.send("不給你刪～")
    
    @commands.command(name="運勢")
    async def luckyFunction(self,ctx):
        if ctx.author.name in lucky and ctx.author.name != "hello_i_am_87":
            await ctx.send('你今天抽過了,是'+lucky[ctx.author.name]+'！！')
        else:
            lucky_str=""
            random_number = random.randint(0, 100)
            if random_number<=5:
                lucky_str='大吉'
            elif random_number<=20:
                lucky_str='中吉'
            elif random_number<=40:
                lucky_str='小吉'
            elif random_number<=60:
                lucky_str='吉'
            elif random_number<=80:
                lucky_str='末吉'
            elif random_number<=95:
                lucky_str='小凶'
            else:
                lucky_str='大凶'
            lucky[ctx.author.name]=lucky_str
            await ctx.send('你今天的運勢是'+lucky_str+'！！')
    @commands.command(name="运势")
    async def luckyFunction_2(self,ctx):
        if ctx.author.name in lucky:
            await ctx.send('你今天抽过了,是'+lucky[ctx.author.name]+'！！')
        else:
            lucky_str=""
            random_number = random.randint(0, 100)
            if random_number<=5:
                lucky_str='大吉'
            elif random_number<=20:
                lucky_str='中吉'
            elif random_number<=40:
                lucky_str='小吉'
            elif random_number<=60:
                lucky_str='吉'
            elif random_number<=80:
                lucky_str='末吉'
            elif random_number<=95:
                lucky_str='小凶'
            else:
                lucky_str='大凶'
            lucky[ctx.author.name]=lucky_str
            await ctx.send('你今天的运势是'+lucky_str+'！！')
    @commands.command(name="指令") 
    async def instructions(self,ctx):
        await ctx.send("!排隊、!目前、!取消、!抓、!戳、!運勢")
    
    @commands.command(name="取消") 
    async def cancle(self,ctx):
        if not nameList:
            await ctx.send("目前沒有人排隊！！")
        else:
            boolFounded = False
            for index, strName in enumerate(nameList):
                if ctx.author.display_name == strName:
                    boolFounded = True
                    del nameList[index]
                    await ctx.send("已取消排隊。")
                    break
            if boolFounded == False:
                await ctx.send("你還沒有排隊！！")
    @commands.command(name="退出") 
    async def quitlist(self,ctx,strName:str = " "):
        if ctx.author.name != "hello_i_am_87":
            await ctx.send("不給你用！！")
            return
        if not nameList:
            await ctx.send("目前沒有人排隊！！")
        else:
            boolFounded = False
            for index, strName in enumerate(nameList):
                if ctx.author.display_name == strName:
                    boolFounded = True
                    del nameList[index]
                    await ctx.send("已取消 "+strName+" 排隊。")
                    break
            if boolFounded == False:
                await ctx.send(strName+" 不在隊伍裡...")
    @commands.command(name="抽籤")
    async def rd(self,ctx):
        if len(ctx.message.content.split(" ")) ==2 : 
            str1 = ctx.message.content.split(" ")[1]

            intNum=random.randint(1, 10)
            await ctx.send("抽籤中...")
            await asyncio.sleep(3)
            if intNum <5:
                await ctx.send(str1 + " 通過！")
            else:
                await ctx.send(str1 + " 不通過...")
        else:
            await ctx.send("指令不對...")

################################指令區################################


################################bot################################

################################指令區################################


csharp_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
csharp_socket.connect(('127.0.0.1', 15003))
twitchBot=TwitchBot()
twitchBot.run()
csharp_socket.close()



        