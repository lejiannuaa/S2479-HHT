package com.hola.bs.core;

import java.net.InetAddress;
import java.net.UnknownHostException;
import java.util.Date;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.xsocket.connection.INonBlockingConnection;

import com.hola.bs.bean.CommandContainer;
import com.hola.bs.bean.RequestValidation;
import com.hola.bs.bean.Response;
import com.hola.bs.bean.UseVerify;
import com.hola.bs.core.exception.ProcessUnitNotException;
import com.hola.bs.core.exception.RequestRepeatException;
import com.hola.bs.core.exception.UserTimeoutException;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.DateUtils;

/**
 * Socket命令处理器 该对象负责分解Socket传入的命令，并将其转发到不同处理单元
 * 
 * @author S1608
 * 
 */
public class Command {
	public Log log = LogFactory.getLog(getClass());
	public Log log2 = LogFactory.getLog("sysReceiveLog");
	private UserContainer us = null;

	// 用户验证对象
	private UseVerify uv = null;

	/**
	 * 请求验证对象
	 */
	private RequestValidation rv = null;
	/**
	 * 保存命令编号和处理单元的关系
	 */
	public ConcurrentHashMap<String, ProcessUnit> unitMap;

	/**
	 * 接受请求并返回处理结果
	 * 
	 * @param strCommand:请求字符串
	 * @param ip:请求设备所在的IP
	 * @throws Exception 
	 */
	public String accept(String strCommand, String ip,INonBlockingConnection nbc) throws UserTimeoutException {
		//考虑接收head档信息
		
		boolean flag = isHead(strCommand,nbc);
//		return new Response(strCommand,BusinessService.successcode,"").toString();
		// 如果是请求信息的体部 flag = false
		if(!flag){
			String socketId = nbc.getId();
			CommandContainer commandContainer = Init.commContainerMap.get(socketId);
			try{
				if (commandContainer != null) {
					// 声明一个变量 记录目前接收信息的长度
					int curr_size = strCommand.length();
				// 1 计算接收信息的长度，相加
				// System.out.println("socketId = "+socketId +"
				// 对象是否为空："+(commandContainer==null));
				curr_size = curr_size
				+ commandContainer.getCurrSize();
				commandContainer.setCurrSize(curr_size);
				// 2 字符串拼接		
				StringBuffer currBuffer = commandContainer
					.getCurrBuffer();
				currBuffer.append(strCommand);
				commandContainer.setCurrBuffer(currBuffer);
				
//				System.out.println(("处理指令为：")+currBuffer.toString());
//				System.out.println(currBuffer.toString().length());
				// 3 判断当前接收信息的总长度是否等于头档信息中的总长度 如果小于，则继续等待接收
				// System.out.println(curr_size + "
				// 总长度为："+totalSize);
//					if (curr_size < commandContainer.getTotalSize()) {
					if (commandContainer.getCurrSize() < commandContainer.getTotalSize()) {
					// 如果小于总长度，则使用指令收集器储存当前指令信息
//						commandContainer.setCurrSize(curr_size);
//						commandContainer.setCurrBuffer(currBuffer);
						Init.commContainerMap.put(socketId,commandContainer);
						return null;
					}else{
						String finalStrCommand = "";
						String totalBufferCommand = commandContainer.getCurrBuffer().toString();
						log.info("successfully received!!! From ip :"+ nbc.getRemoteAddress().getHostAddress()+ ". Total Command is "+totalBufferCommand+", receive time : "+DateUtils.string2TotalTime(new Date())+" current socketId is : "+nbc.getId());
						finalStrCommand =  totalBufferCommand+";userip=" + ip + ";storeip=" + InetAddress.getLocalHost().getHostAddress().toString();
						// 分解传入的请求
						String guid = commandContainer.getGuid();
//						Init.commContainerMap.remove(socketId);
						Request request = new Request(finalStrCommand);
						request.getParameterMap().put("guid", guid);
						request.getParameterMap().put("requestValue", totalBufferCommand);
					
					// 获得用户名
						String name = request.getParameter(Request.USR);
					//验证是否是重复请求
						if ((!guid.equals("00000000000000000000000000000000"))) {
						// 抛出异常的方式中止当前操作
							List<Map> list = validateRequest(guid,uv.getUserContainer().getUser(name).getStore());
							if (list != null && list.size() > 0) {
								String memo = list.get(0).get("memo").toString();
								if (memo == null || memo.length() == 0)
									memo = "";
								throw new RequestRepeatException(memo);
							}
						}
					

					// 用户是否已登录
						if (uv.isAlreadyLogin(name)) {
						// 用户强制登陆
							
							if (request.getRequestID().equals("login2")) {
								us.register(request);
								return dispatch("login", request);
							
							// 验证是否来自相同用户名，不同IP的登录请求
							} else if (!uv.sameUserDiffIP2(name, ip)) {
								log.error("用户登录出现异常，请检查现场情况以及IP地址是否变动");
								return new Response("login", BusinessService.userAlreadyLogin, "该用户已经登陆，是否要再次登陆？").toString();
							
							//用户是否已超时
							} else if (uv.isTimeout(name)) {
								us.logout(name);// 用户如已超时，则注销用户
								return new Response("用户登录已超时", BusinessService.errorcode, "用户登录已超时！").toString();
						
							//允许用户执行操作
							} else {
								log2.info("start: connectionId is: "+ nbc.getId() + ", time: "+DateUtils.string2TotalTime(new Date()));
								return dispatch(request.getRequestID(), request);
							}
						} else {
						// 用户登录请求
							if (request.getRequestID().equals("login")) {
								us.register(request);
								log2.info("start: connectionId is: "+ nbc.getId() + ", time: "+DateUtils.string2TotalTime(new Date()));
								return dispatch(request.getRequestID(), request);
							}else{
								return new Response("login", BusinessService.errorcode, "用户尚未登录！").toString();
							}
						}
					}
				}
				return "";
			}catch (ProcessUnitNotException e) {
				e.printStackTrace();
				return new Response("不存在的处理指令", BusinessService.errorcode, "不存在的处理指令！").toString();
			}catch (RequestRepeatException re) {
				Response response = new Response();
				response.setResponse("");
				response.setCode(BusinessService.successcode);
				response.setDesc(re.getMessage());
				return response.toString();
			}catch(UnknownHostException ue){
				ue.printStackTrace();
				return "";
			}
		}else{
			return "";
		}
	}	

	private boolean isHead(String strCommand,INonBlockingConnection nbc) {
		// TODO Auto-generated method stub
		log.info("isHead解析指令======================================>"+strCommand);
		// 判断接收的是不是头档信息
		// 头档信息格式例如0000009567，其中，前面4个0表示业务含义，后6位表示发送的字符串的长度，不足补领
		// 再从第10位开始截取后面的部分，此部分为GUID，需要做相应的处理
		log.info("-----------------------------------------------------------strCommand == null"+strCommand == null);
		log.info("-----------------------------------------------------------strCommand == ''"+strCommand == "");
		if("".equals(strCommand) || strCommand.length()<4){
			return false;
		}else{
//			if (strCommand.substring(0, 4).equalsIgnoreCase("XXXX")||strCommand.substring(0, 4).equalsIgnoreCase("0000")) {
			if (strCommand.substring(0, 4).equalsIgnoreCase("XXXX")) {
				String socketId = nbc.getId();
				// System.out.println("Head 信息为："+reczivedString);
				String requestLength = strCommand.substring(4, 10);
				//判断是否全数字
				if(!isNumber(requestLength)){
					log.info("-----------------------------------------------------------requestLength ="+requestLength +"由数字组成。");
					int totalSize = Integer.parseInt(requestLength);
					// System.out.println(Init.commContainerMap==null);
					String guid = strCommand.substring(10);
					if (Init.commContainerMap.get(socketId) == null) {
						CommandContainer commandContainer = new CommandContainer(
								totalSize, new StringBuffer(), 0, guid);
						Init.commContainerMap.put(socketId,
								commandContainer);
					}
					return true;
				}
				log.info("-----------------------------------------------------------requestLength ="+requestLength +"不是数字组成。");
				return false;
			} else{
				return false;
			}
		}
	}

	/**
	 * 转发请求到实际的处理单元
	 * 
	 * @param id:指令标示
	 * @param request:命令字符串
	 * @throws ProcessUnitNotException
	 *             指令对应的处理单元不存在
	 */
	private String dispatch(String id, Request request) throws ProcessUnitNotException {
		
		//检查用户是否正常登陆
//		UserState user = us.getUser(request.getParameter(Request.USR));
//		if(user.getStore()==null){
//			return new Response("login", BusinessService.errorcode, "用户登录失败，请重新登录！").toString();
//		}
		
//		System.out.println(id + "==" + unitMap.get(id));
		
//		synchronized(unitMap){
			if (unitMap.containsKey(id)){
				return unitMap.get(id).process(request);
			}else{
				throw new ProcessUnitNotException("指令[" + id + "]对应的处理单元不存在！");
			}
//		}
	}


	/**
	 * 将新的请求插入到数据表里
	 * @param guid
	 * @param receivedRequest
	 * @return
	 * author: S2139
	 * 2012 Oct 30, 2012 6:15:26 PM
	 * @throws Exception 
	 */
	public int insertNewRequest(String guid, String receivedRequest) throws Exception{
		return rv.insertNewRequest(guid, receivedRequest);
	}
	
	/**
	 * 验证是否是重复提交的请求，方法是，在请求时获取GUID，在数据库中做判断
	 * @param guid
	 * @return
	 * author: S2139
	 * 2012 Oct 30, 2012 6:13:53 PM
	 */
	public List<Map> validateRequest(String guid,String schema){
		String s = schema;
		if(schema ==null || schema.length()==0){
			s = "13104";
		}
		return rv.validateRequest(guid,s);
	}
	
	
	private boolean isNumber(String requestLength){
		Pattern pattern = Pattern.compile("^[0-9]+$");
		Matcher matcher = pattern.matcher(requestLength);
		log.info("-----------------------------------------------------------isNumber ="+matcher.find());
		return matcher.find();
	}

	public synchronized ConcurrentHashMap<String, ProcessUnit> getUnitMap() {
		return unitMap;
	}

	public synchronized void setUnitMap(ConcurrentHashMap<String, ProcessUnit> unitMap) {
		this.unitMap = unitMap;
	}

	public UserContainer getUs() {
		return us;
	}

	public void setUs(UserContainer us) {
		this.us = us;
	}

	public UseVerify getUv() {
		return uv;
	}

	public void setUv(UseVerify uv) {
		this.uv = uv;
	}

	public RequestValidation getRv() {
		return rv;
	}

	public void setRv(RequestValidation rv) {
		this.rv = rv;
	}

	public static void main(String[] args) {
//		Pattern pattern = Pattern.compile("^[0-9]+$");
//		String s="04370-";
//		Matcher matcher = pattern.matcher(s);
//		System.out.println(matcher.find());
		System.out.print(Long.parseLong("01565a1bff3c4ac",16));  
	}

}
