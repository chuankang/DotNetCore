require.config(
    {
        //基础路径
        baseUrl: "/js",
        //版本
        urlArgs: 'v=180524',
        //配置路径
        paths: {
            'Vue': 'Vue',
            'AJAX': 'components/ajax'
        },
        shim: {
            'Vue': {
                exports: 'Vue'
            }
            
        }
    }
);