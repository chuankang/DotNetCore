require(["components/config"],
    function() {
        require(['Vue'],
            function(Vue) {
                var app = new Vue({
                    el: '#app',
                    data: {
                        message: 'Hello Vue!'
                    }
                })
            }
        )
    })