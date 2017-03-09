package com.hola.bs.pool;

import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.charset.Charset;
import java.util.Date;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.xsocket.connection.INonBlockingConnection;

import com.hola.bs.core.Command;
import com.hola.bs.core.Init;
import com.hola.bs.core.exception.UserTimeoutException;
import com.hola.bs.socket.TCPClient;
import com.hola.bs.util.DateUtils;

public class Runner implements Runnable {

	public Log log = LogFactory.getLog(Runnable.class);
	

	private int i;
	private Command command;
	private INonBlockingConnection nbc;
	private boolean isClientTest = false;

	public Runner(int i) {
		this.i = i;
		this.isClientTest = true;
	}

	public Runner(Command command, INonBlockingConnection nbc) {
		this.command = command;
		this.nbc = nbc;
	}

	public void run() {
		if (isClientTest) {// clientTest
			TCPClient t;
			try {
				t = new TCPClient("192.168.1.100", 1978);
				t.sendMsg("=" + i);
			} catch (IOException e) {
				e.printStackTrace();
			}
		} else {// BridgeServer Reveive Socket
			Init.startMills = System.currentTimeMillis();
			System.out.println("当前connectionId: "+nbc.getId()+", 计时开始时间："+DateUtils.DateFormatToString(DateUtils.getSystemDate(),
					 "yyyy-MM-dd HH:mm:ss.SSS"));
			if (nbc.isOpen()) {
// 				System.out.println("===run thread==="+nbc.getId());
//				String socketId = nbc.getId();
				byte[] data ;
				String result = "";
//				int totalSize = 0;
				// StringBuffer receivedBuffer = new StringBuffer();
				try {
					data = nbc.readBytesByLength(nbc.available());// 接受字节数据 
					ByteBuffer b = ByteBuffer.wrap(data);// 组装成字节缓存
					String receivedString = Charset.forName("UTF-8")
							.newDecoder().decode(b).toString();// 解析缓存成为字符串
					// boolean needCache = false;//设个变量，确认是否需要缓存，默认不需要
					log.info("receive data, connectionId is: "+ nbc.getId() + ", time: "+DateUtils.string2TotalTime(new Date()) + ", thread_no : "+this.toString());
					log.info("connectionId = " +nbc.getId()+" thread_no : "+this.toString()+", 接收数据长度："+receivedString.length()+", 接收数据：" + receivedString);
					 result = command.accept(receivedString,nbc.getRemoteAddress().getHostAddress(),nbc);
										
					 String timeStr =
					 DateUtils.DateFormatToString(DateUtils.getSystemDate(),
					 "yyyy-MM-dd HH:mm:ss.SSS");
					 log.info("处理数据完成，"+" ,处理返回值 = "+result+", connectionId is: "+ nbc.getId() + ", time: "+DateUtils.string2TotalTime(new Date()) + " thread_no : "+this.toString() );
//					 System.out.println("当前connectionId: "+nbc.getId()+", 计时结束时间："+timeStr);
//					 System.out.println("****************************************************************************************");
//					 System.out.println("\n");
					 // System.out.println("输出到：" +timeStr+" "+
					if(result!=null && result.length()>0){
						nbc.write(result);// 写出到客户端
						log.info("send response: "+result+", connectionId is: "+ nbc.getId() + ", time: "+DateUtils.string2TotalTime(new Date())+" thread_no : "+this.toString());
					}else if ("".equals(result)){
						result = "8C61855381974fd8A8ED3021C906BB57";//此内容需和厂商约定之
						nbc.write(result);
						log.info("send response: "+result+", connectionId is: "+ nbc.getId() + ", time: "+DateUtils.string2TotalTime(new Date())+" thread_no : "+this.toString());
					}
				} catch (IOException e1) {
					e1.printStackTrace();
				} catch (UserTimeoutException e) {
					// TODO Auto-generated catch block
					result = "你已超时，请重新登录";
					e.printStackTrace();
				} catch (Exception ee) {
					ee.printStackTrace();
				}
			}
		}
	}
	
	public static int length(String value) {
        int valueLength = 0;
        String chinese = "[\u0391-\uFFE5]";
        /* 获取字段值的长度，如果含中文字符，则每个中文字符长度为2，否则为1 */
        for (int i = 0; i < value.length(); i++) {
            /* 获取一个字符 */
            String temp = value.substring(i, i + 1);
            /* 判断是否为中文字符 */
            if (temp.matches(chinese)) {
                /* 中文字符长度为2 */
                valueLength += 2;
            } else {
                /* 其他字符长度为1 */
                valueLength += 1;
            }
        }
        return valueLength;
	}
	
	public static void main(String[] args) {
	String s1 = "request=002;usr=S1777;op=02;bc=C000004370;json={\"root\":{\"config\":{\"type\":\"0\",\"direction\":\"Client->Server\",\"id\":\"00202\"},\"info\":[{\"SKU\":\"000001033\",\"whsl\":\"1\"},{\"SKU\":\"000001850\",\"whsl\":\"1\"},{\"SKU\":\"000003732\",\"whsl\":\"3\"},{\"SKU\":\"000005123\",\"whsl\":\"1\"},{\"SKU\":\"000005160\",\"whsl\":\"1\"},{\"SKU\":\"000009268\",\"whsl\":\"4\"},{\"SKU\":\"000013033\",\"whsl\":\"1\"},{\"SKU\":\"000013545\",\"whsl\":\"1\"},{\"SKU\":\"000018097\",\"whsl\":\"1\"},{\"SKU\":\"000018422\",\"whsl\":\"20\"},{\"SKU\":\"000018714\",\"whsl\":\"24\"},{\"SKU\":\"000018720\",\"whsl\":\"12\"},{\"SKU\":\"000018725\",\"whsl\":\"24\"},{\"SKU\":\"000018728\",\"whsl\":\"1\"},{\"SKU\":\"000018732\",\"whsl\":\"4\"},{\"SKU\":\"000018734\",\"whsl\":\"36\"},{\"SKU\":\"000018736\",\"whsl\":\"24\"},{\"SKU\":\"000019217\",\"whsl\":\"3\"},{\"SKU\":\"000021233\",\"whsl\":\"1\"},{\"SKU\":\"000021240\",\"whsl\":\"1\"},{\"SKU\":\"000021244\",\"whsl\":\"1\"},{\"SKU\":\"000025308\",\"whsl\":\"1\"},{\"SKU\":\"000026946\",\"whsl\":\"10\"},{\"SKU\":\"000026947\",\"whsl\":\"10\"},{\"SKU\":\"000026949\",\"whsl\":\"10\"},{\"SKU\":\"000026950\",\"whsl\":\"10\"},{\"SKU\":\"000026951\",\"whsl\":\"10\"},{\"SKU\":\"000026957\",\"whsl\":\"10\"},{\"SKU\":\"000026962\",\"whsl\":\"10\"},{\"SKU\":\"000030926\",\"whsl\":\"8\"},{\"SKU\":\"000031106\",\"whsl\":\"5\"},{\"SKU\":\"000031169\",\"whsl\":\"7\"},{\"SKU\":\"000031373\",\"whsl\":\"6\"},{\"SKU\":\"000031956\",\"whsl\":\"24\"},{\"SKU\":\"000031957\",\"whsl\":\"1\"},{\"SKU\":\"000032893\",\"whsl\":\"1\"},{\"SKU\":\"000039690\",\"whsl\":\"14\"},{\"SKU\":\"000039703\",\"whsl\":\"1\"},{\"SKU\":\"000040263\",\"whsl\":\"10\"},{\"SKU\":\"000041577\",\"whsl\":\"1\"},{\"SKU\":\"000041663\",\"whsl\":\"2\"},{\"SKU\":\"000042147\",\"whsl\":\"1\"},{\"SKU\":\"000042149\",\"whsl\":\"1\"},{\"SKU\":\"000042225\",\"whsl\":\"1\"},{\"SKU\":\"000042226\",\"whsl\":\"1\"},{\"SKU\":\"000043494\",\"whsl\":\"2\"},{\"SKU\":\"000043495\",\"whsl\":\"1\"},{\"SKU\":\"000043496\",\"whsl\":\"3\"},{\"SKU\":\"000043601\",\"whsl\":\"5\"},{\"SKU\":\"000043699\",\"whsl\":\"12\"},{\"SKU\":\"000051219\",\"whsl\":\"10\"},{\"SKU\":\"000051439\",\"whsl\":\"10\"},{\"SKU\":\"000051446\",\"whsl\":\"10\"},{\"SKU\":\"000052139\",\"whsl\":\"3\"},{\"SKU\":\"000053914\",\"whsl\":\"1\"},{\"SKU\":\"000054196\",\"whsl\":\"4\"},{\"SKU\":\"000054253\",\"whsl\":\"1\"},{\"SKU\":\"000054256\",\"whsl\":\"1\"},{\"SKU\":\"000054328\",\"whsl\":\"5\"},{\"SKU\":\"000054330\",\"whsl\":\"5\"},{\"SKU\":\"000054553\",\"whsl\":\"1\"},{\"SKU\":\"000055497\",\"whsl\":\"4\"},{\"SKU\":\"000056303\",\"whsl\":\"3\"},{\"SKU\":\"000058270\",\"whsl\":\"1\"},{\"SKU\":\"000058279\",\"whsl\":\"12\"},{\"SKU\":\"000058380\",\"whsl\":\"10\"},{\"SKU\":\"000058386\",\"whsl\":\"6\"},{\"SKU\":\"000058404\",\"whsl\":\"4\"},{\"SKU\":\"000063320\",\"whsl\":\"1\"},{\"SKU\":\"000063365\",\"whsl\":\"1\"},{\"SKU\":\"000064835\",\"whsl\":\"1\"},{\"SKU\":\"000066506\",\"whsl\":\"1\"},{\"SKU\":\"000067447\",\"whsl\":\"1\"},{\"SKU\":\"000067454\",\"whsl\":\"1\"},{\"SKU\":\"000067459\",\"whsl\":\"1\"},{\"SKU\":\"000067522\",\"whsl\":\"1\"},{\"SKU\":\"000068158\",\"whsl\":\"10\"},{\"SKU\":\"000069037\",\"whsl\":\"7\"},{\"SKU\":\"000069152\",\"whsl\":\"1\"},{\"SKU\":\"000069695\",\"whsl\":\"1\"},{\"SKU\":\"000069885\",\"whsl\":\"1\"},{\"SKU\":\"000070033\",\"whsl\":\"5\"},{\"SKU\":\"000070710\",\"whsl\":\"6\"},{\"SKU\":\"000071241\",\"whsl\":\"6\"},{\"SKU\":\"000071252\",\"whsl\":\"20\"},{\"SKU\":\"000071255\",\"whsl\":\"10\"},{\"SKU\":\"000071262\",\"whsl\":\"18\"},{\"SKU\":\"000071312\",\"whsl\":\"1\"},{\"SKU\":\"000071386\",\"whsl\":\"1\"},{\"SKU\":\"000071537\",\"whsl\":\"10\"},{\"SKU\":\"000071572\",\"whsl\":\"10\"},{\"SKU\":\"000072071\",\"whsl\":\"1\"},{\"SKU\":\"000072123\",\"whsl\":\"4\"},{\"SKU\":\"000072208\",\"whsl\":\"18\"},{\"SKU\":\"000072209\",\"whsl\":\"18\"},{\"SKU\":\"000072237\",\"whsl\":\"5\"},{\"SKU\":\"000072238\",\"whsl\":\"10\"},{\"SKU\":\"000072476\",\"whsl\":\"1\"},{\"SKU\":\"000073084\",\"whsl\":\"4\"},{\"SKU\":\"000073085\",\"whsl\":\"18\"},{\"SKU\":\"000073086\",\"whsl\":\"10\"},{\"SKU\":\"000073087\",\"whsl\":\"5\"},{\"SKU\":\"000073323\",\"whsl\":\"5\"},{\"SKU\":\"000073325\",\"whsl\":\"5\"},{\"SKU\":\"000073855\",\"whsl\":\"3\"},{\"SKU\":\"000073872\",\"whsl\":\"10\"},{\"SKU\":\"000073912\",\"whsl\":\"5\"},{\"SKU\":\"000073913\",\"whsl\":\"15\"},{\"SKU\":\"000073914\",\"whsl\":\"10\"},{\"SKU\":\"000073916\",\"whsl\":\"10\"},{\"SKU\":\"000073922\",\"whsl\":\"10\"},{\"SKU\":\"000073923\",\"whsl\":\"10\"},{\"SKU\":\"000074205\",\"whsl\":\"6\"},{\"SKU\":\"000074307\",\"whsl\":\"1\"},{\"SKU\":\"000074314\",\"whsl\":\"1\"},{\"SKU\":\"000074695\",\"whsl\":\"2\"},{\"SKU\":\"000074702\",\"whsl\":\"10\"},{\"SKU\":\"000075210\",\"whsl\":\"5\"},{\"SKU\":\"000075376\",\"whsl\":\"3\"},{\"SKU\":\"000075499\",\"whsl\":\"6\"},{\"SKU\":\"000075915\",\"whsl\":\"20\"},{\"SKU\":\"000075916\",\"whsl\":\"1\"},{\"SKU\":\"000105537\",\"whsl\":\"3\"}],\"diff\":[]}}";
		System.out.println(s1);
		System.out.println(s1.getBytes().length);
	}
}
