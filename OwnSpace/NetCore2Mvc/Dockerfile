#基于 `microsoft/aspnetcore:2.0` 来构建我们的镜像
FROM microsoft/aspnetcore:2.0

#拷贝项目publish文件夹中的所有文件到 docker容器中的publish文件夹中  
COPY . /publish

#设置工作目录为 `/publish` 文件夹，即容器启动默认的文件夹
WORKDIR /publish

#设置Docker容器对外暴露80端口
EXPOSE 80

#使用`dotnet NetCore2Mvc.dll`来运行应用程序
ENTRYPOINT ["dotnet", "NetCore2Mvc.dll"]
