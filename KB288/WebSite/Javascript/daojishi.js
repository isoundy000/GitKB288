function daojishi(divname, endtime,servertime) {
    var interval = 1000;
    var ttime = new Date(endtime.replace(/-/g, '/'));
    var otime = new Date(servertime.replace(/-/g, '/'));
    var leftTime = ttime.getTime() - otime.getTime();
    var leftsecond = parseInt(leftTime / interval);
    function ShowCountDown() {
        var day = Math.floor(leftsecond / (60 * 60 * 24));
        var hour = Math.floor((leftsecond - day * 24 * 60 * 60) / 3600);
        var minute = Math.floor((leftsecond - day * 24 * 60 * 60 - hour * 3600) / 60);
        var second = Math.floor(leftsecond - day * 24 * 60 * 60 - hour * 3600 - minute * 60);
        var cc = document.getElementById(divname);
        var text = "";
        if (day > 0)
            text += day + "天";
        if (hour > 0)
            text += hour + "小时";
        if (minute > 0)
            text += minute + "分";
        if (second > 0)
            text += second + "秒";
        if (leftsecond > 0) {
            cc.innerHTML = text;
            leftsecond--;
        }
        else {
            cc.innerHTML = "0秒";
            location.reload(true);
        }
        setTimeout(ShowCountDown, interval);
    }
    setTimeout(ShowCountDown, interval);
}
