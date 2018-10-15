define(function (require, exports, module) {
    var $ = require('jquery');
    //根据关键的几个参数统一创建ajax对象
    function create(_url, _method, _data, _async, _dataType, _contentType, _cache) {
        // //添加随机数
        // if (_url.indexOf('?') > -1) {
        //     _url = _url + '&rnd=' + Math.random();
        // } else {
        //     _url = _url + '?rnd=' + Math.random();
        // }

        // //为请求添加ajax标识，方便后台区分ajax和非ajax请求
        // _url += '&_ajax=1';

        //返回jquery创建的ajax对象，以便外部拿到这个对象以后可以通过
        //.done .fail .always来添加回调
        //这么做是为了保留jquery ajax中好用的部分
        return $.ajax({
            url: _url,
            dataType: _dataType,
            async: _async,
            method: _method,
            data: _data,
            contentType: _contentType,
            cache: _cache
        });
    }
    //ajax就是本组件全局唯一的实例，它的实例方法通过后面的循环代码添加
    //methods对象配置ajax各个实例方法的参数：
    //name: 方法名称
    //method: http请求方法，get or post
    //async: 发送请求时是否异步
    //dataType: 返回的数据类型，html or json
    var ajax = {},
        methods = [
            {
                name: 'html',
                method: 'get',
                async: true,
                dataType: 'html'
            },
            {
                name: 'get',
                method: 'get',
                async: true,
                dataType: 'json',
                cache: false,
                contentType: 'application/x-www-form-urlencoded'
            },
            {
                name: 'getStringify',
                method: 'get',
                async: true,
                dataType: 'json',
                contentType: 'application/json'
            },          
            {
                name: 'post',
                method: 'post',
                async: true,
                dataType: 'json',
                cache: false,
                contentType: 'application/x-www-form-urlencoded'
            },
            {
                name: 'postStringify',
                method: 'post',
                async: true,
                dataType: 'json',
                contentType: 'application/json'
            },      
            {
                name: 'syncGet',
                method: 'get',
                async: false,
                dataType: 'json'
            },
            {
                name: 'syncPost',
                method: 'post',
                async: false,
                dataType: 'json'
            }
        ];
    //由于二次封装需要对外提供的每个实例方法创建ajax的逻辑是相同的
    //所以通过这种方式统一定义各个实例方法
    //关键代码为下面代码中的那个立即调用的函数
    //它返回了一个新的闭包函数作为实例方法
    for (var i = 0, l = methods.length; i < l; i++) {
        //对象中使用方括号可以传入变量，但是点运算符不能传入变量
        ajax[methods[i].name] = (function (i) {
            return function () {
                /**
                 * 每个实例方法接收三个参数
                 * 第一个表示要请求的地址
                 * 第二个表示要提交到后台的数据，是一个object对象，如{param1: 'value1'}
                 * 第三个表示后台返回的数据类型，最最常用的就是html or json，绝大部分情况下这个参数不用传，会使用methods里面定义的dataType
                 */
                var _url = arguments[0],
                    _data = arguments[1],
                    _dataType = arguments[2] || methods[i].dataType;

                return create(_url, methods[i].method, _data, methods[i].async, _dataType, methods[i].contentType, methods[i].cache);
            }
        })(i);
    }
    return ajax;
});