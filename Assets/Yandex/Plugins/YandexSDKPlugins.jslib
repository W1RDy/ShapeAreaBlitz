mergeInto(LibraryManager.library, {

  // AuthorizeExtern: function (){
  //   auth();
  // },

  RateGame: function (){
    rate();
  },

  SaveExtern: function (data){
    var dataString = UTF8ToString(data);
    var myObj = JSON.parse(dataString); 
    player.setData(myObj);
  },

  LoadExtern: function (){
    load();
  },

  ShowAdv: function(){
    ysdk.adv.showFullscreenAdv({
      callbacks: {
        onClose: function(wasShown) {
          myGameInstance.SendMessage('AudioManager', 'PauseStartAudio', 'true');
        },
        onError: function(error) {
          // some action on error
        }
      }
    })
  },

  MultiplyCoinsForAdvExtern: function(value){
    var isRewarded = false;
    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: function(){
          console.log('Video ad open.');
        },
        onRewarded: function(){
          console.log('Rewarded!');
          myGameInstance.SendMessage('Rewarder', 'MultiplyReward', value)
          isRewarded = true;
        },
        onClose: function(){
          console.log('Video ad closed.');
          myGameInstance.SendMessage('AudioManager', 'PauseStartAudio', 'true');
          if (isRewarded){
            myGameInstance.SendMessage('AudioManager', 'PlaySound', 'Doubler');
          }
        }, 
        onError: function(e){
          console.log('Error while open video ad:', e);
        }
      }
    })
  },

  AddSecondLifeExtern: function(){
    var isRewarded = false;
    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: function(){
          console.log('Video ad open.');
        },
        onRewarded: function(){
          console.log('Rewarded!');
          myGameInstance.SendMessage('Reborner', 'Reborn');
          isRewarded = true;
        },
        onClose: function(){
          console.log('Video ad closed.');
          myGameInstance.SendMessage('AudioManager', 'PauseStartAudio', 'true');
          if (isRewarded){
            myGameInstance.SendMessage('GameService', 'ContinueGame');
            myGameInstance.SendMessage('AudioManager', 'PlaySound', 'Heal');
          }
        }, 
        onError: function(e){
          console.log('Error while open video ad:', e);
        }
      }
    })
  },

  SaveScoreInLeaderboardExtern: function(score)
  {
    saveInLeaderboard(score);
  },

  PurchaseRecommendedSkinForADVExtern: function()
  {
    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: function(){
          console.log('Video ad open.');
        },
        onRewarded: function(){
          console.log('Rewarded!');
          myGameInstance.SendMessage('SkinManager', 'BuyRecommendedSkin');
        },
        onClose: function(){
          console.log('Video ad closed.');
          myGameInstance.SendMessage('AudioManager', 'PauseStartAudio', 'true');
        }, 
        onError: function(e){
          console.log('Error while open video ad:', e);
        }
      }
    })
  },

  GetLanguageExtern: function()
  {
    var lang = ysdk.environment.i18n.lang;
    var bufferSize = lengthBytesUTF8(lang) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(lang, buffer, bufferSize);
    return buffer;
  },

  PlayerIsInitialized: function()
  {
    var isInitialized = String(player != undefined);
    var bufferSize = lengthBytesUTF8(isInitialized) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(isInitialized, buffer, bufferSize);
    return buffer;
  },

});