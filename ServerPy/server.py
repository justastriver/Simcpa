# coding=utf-8
import SocketServer
import WPMessage_pb2

from SocketServer import StreamRequestHandler as SRH
from time import ctime

host = '192.168.2.13'
port = 6666
addr = (host,port)

class Servers(SRH):
    def get_data(self):
        msg = WPMessage_pb2.Message()
        msg.header.cmd = WPMessage_pb2.Header.PRINTERTASK
        msg.header.seq = 123
        msg.header.restaurant_id = 666
        receipt = msg.data.receipts.add()
        receipt.printer_id = 1
        receipt.order_id = 1111
	receipt.print_num = 2
        receipt.head.title = u"趣吃饭（1号）"
        receipt.head.subtitle = u"（商户）"
        receipt.head.way = u"到店堂食"
        receipt.head.password = "12345"
        food = receipt.body.foods.add()
        food.name = u"西红柿鸡蛋套餐"
        food.price = "10.0"
        food.num = 3
        slave_food = food.slavefoods.add()
        slave_food.name = u"西红柿"
        slave_food.price = "5"
        slave_food.num = 1
        slave_food = food.slavefoods.add()
        slave_food.name = u"鸡蛋羹"
        slave_food.price = "5"
        slave_food.num = 2
        receipt.tail.restaurant_name = u"Andrew's 茶餐厅"
        receipt.tail.totalprice = "18.5"
        receipt.tail.orderno = "NO.102233"
        receipt.tail.phone = "13522712205"
        receipt.tail.time = "2016-04-01 12:00:00"
        data = msg.SerializeToString()
        #print msg, 'data len: ',len(data)
        return data
        
    def handle(self):
        resp = WPMessage_pb2.Message()
        req = WPMessage_pb2.Message()
        print 'got connection from ',self.client_address
        data = self.get_data() 
        self.request.send(data)
        while True:
            print 'waiting client response...'
            data = self.request.recv(1024)
            if not data: 
                break
            print "recv from ", self.client_address[0],len(data)
            resp.ParseFromString(data)
            print resp
            #response
	    if resp.header.cmd is WPMessage_pb2.Header.PRINTERTASK:
		continue
            req.header.cmd=resp.header.cmd
            req.header.seq=resp.header.seq
            req.header.restaurant_id=resp.header.restaurant_id
            req.data.result.code=WPMessage_pb2.Result.SUCC
            req.data.result.message="response of server,hi"
            req_data = req.SerializeToString()
            self.request.send(req_data)
            
            #self.wfile.write('connection %s:%s at %s succeed!' % (host,port,ctime()))
print 'server is running....'
msg = WPMessage_pb2.Message()
msg.header.cmd = WPMessage_pb2.Header.HEARTBEAT
msg.header.seq = 123
msg.header.restaurant_id = 666
receipt = msg.data.receipts.add()
receipt.printer_id = 1
receipt.order_id = 1111
receipt.print_num= 1
receipt.head.title = u"趣吃饭（1号）"
receipt.head.subtitle = u"（商户）"
receipt.head.way = u"到店堂食"
receipt.head.password = "12345"
food = receipt.body.foods.add()
food.name = u"西红柿鸡蛋套餐"
food.price = "10.0"
food.num = 3
slave_food = food.slavefoods.add()
slave_food.name = u"西红柿"
slave_food.price = "5"
slave_food.num = 1
slave_food = food.slavefoods.add()
slave_food.name = u"鸡蛋羹"
slave_food.price = "5"
slave_food.num = 2
receipt.tail.restaurant_name = u"Andrew's 茶餐厅"
receipt.tail.totalprice = "18.5"
receipt.tail.orderno = "NO.102233"
receipt.tail.phone = "13522712205"
receipt.tail.time = "2016-04-01 12:00:00"
data = msg.SerializeToString()
#print msg, 'data len: ',len(data)

#msg = WPMessage_pb2.Message()
#msg.header.cmd = WPMessage_pb2.Header.HEARTBEAT
#msg.header.seq = 123
#msg.header.restaurant_id = 666
#print msg
#data = msg.SerializeToString()
#print len(data)
#msg2 = WPMessage_pb2.Message()
#msg2.ParseFromString(data)
#print "parse:",msg2
server = SocketServer.ThreadingTCPServer(addr,Servers)
server.serve_forever()
