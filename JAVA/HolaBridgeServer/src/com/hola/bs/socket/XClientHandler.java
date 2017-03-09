package com.hola.bs.socket;

import java.io.IOException;
import java.nio.BufferUnderflowException;
import java.nio.channels.ClosedChannelException;

import org.xsocket.MaxReadSizeExceededException;
import org.xsocket.connection.IConnectHandler;
import org.xsocket.connection.IDataHandler;
import org.xsocket.connection.IDisconnectHandler;
import org.xsocket.connection.INonBlockingConnection;

/**
 * 客户端定义数据的处理类 
 * @author S1608
 *
 */
public class XClientHandler implements IDataHandler ,IConnectHandler ,IDisconnectHandler {  
  
    /** 
     * 连接的成功时的操作 
     */  
    
    public boolean onConnect(INonBlockingConnection nbc) throws IOException,  
            BufferUnderflowException, MaxReadSizeExceededException {  
        String  remoteName=nbc.getRemoteAddress().getHostName();  
        System.out.println("remoteName "+remoteName +" has connected ！");  
       return true;  
    }  
    /** 
     * 连接断开时的操作 
     */  
     
    public boolean onDisconnect(INonBlockingConnection nbc) throws IOException {  
        // TODO Auto-generated method stub  
       return false;  
    }  
    
    /** 
     *  
     * 接收到数据时候的处理 
     */  
   
    public boolean onData(INonBlockingConnection nbc) throws IOException,  
            BufferUnderflowException, ClosedChannelException,  
            MaxReadSizeExceededException {  
         String data=nbc.readStringByDelimiter("|");  
         nbc.write("--|Client:receive data from server sucessful| -----");  
         nbc.flush();  
         System.out.println(data);  
         return true;  
    }  
  
}  
