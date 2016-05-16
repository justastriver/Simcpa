# Simcpa
SIMple Cloud Printer Agent

#### Simcpa 为Windows平台智能打印客户端，通过使用Google Protocal buffer作为接口，能够方便快捷地与服务端接入，执行云打印任务。
##支持Windows的打印机打印
1. 支持USB，串口，并口及网口（Wifi无线和有线网络）的多种接口打印机支持
2. 支持Wiindows驱动打印
3. 增加配置打印机，测试打印等功能
4. 增加与服务器通信配置
5. 增加对图片打印机功能的支持

##接口（详细参见WPMessage.proto）
1. 心跳
2. 同步打印机配置
3. 打印任务