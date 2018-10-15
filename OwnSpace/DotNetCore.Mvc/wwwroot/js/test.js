require(["components/config"],
    function () {
        require(['AJAX', 'Vue'],
            function (AJAX, Vue) {
                var app = new Vue({
                    el: '#app',
                    data: {
                        message: 'Hello Vue!'
                    }
                });

                var app2 = new Vue({
                    el: '#app-4',
                    data: {
                        todos: {}
                    },
                    //在服务器端渲染期间不被调用
                    mounted: function () {
                        this.GetTest();
                    },
                    // 自动绑定为 Vue 实例
                    methods: {
                        GetTest: function () {
                            AJAX.get('/Home/GetTest', { 'Id': 1 }).done((res) => {
                                this.todos = res.data;
                            });
                        }
                    }
                });


            }

        );
    })