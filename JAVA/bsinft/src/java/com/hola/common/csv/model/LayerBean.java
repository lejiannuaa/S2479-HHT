package com.hola.common.csv.model;

import java.io.Serializable;
import java.util.LinkedList;
import java.util.List;

public class LayerBean implements Serializable {
	private static final long serialVersionUID = 1L;
	public static final String BOF = "BOF";
	public static final String EOF = "EOF";
	public static final String DTA = "DTA";
	// 来源系统
	private String sysName;
	// 电文代码
	private String msgCode;
	// 层代码
	private String layCode;
	// 交换资料名称
	private String chgname;
	// 数据
	private List<String[]> dataList = new LinkedList<String[]>();
	// 条数
	private int size;

	private int dataSize;

	public int getSize() {
		return size;
	}

	public void setSize(int size) {
		this.size = size;
	}

	public LayerBean(String[] layData, String separator) {
		this.setLayer(layData, separator);
	}

	public LayerBean() {
	}

	/**
	 * 解析数据
	 * 
	 * @file: LayerBean.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-28
	 * @param layData
	 * @param separator
	 */
	private void setLayer(String[] layData, String separator) {
		for (String data : layData) {
			if (data.startsWith(BOF + separator)) {
				setHead(data.split(separator));
				continue;
			}
			if (data.startsWith(DTA + separator)) {
				// 去掉DTA
				data = data.substring(4);
				
//				data = data.trim();
				String [] dataArr = splitForCsv(data , separator);// data.split(separator);
					
				this.dataList.add(dataArr);
				continue;
			}
			if (data.startsWith(EOF + separator)) {
				setFooter(data.split(separator));
				break;
			}
		}
	}
	/**
	 * 数据分割算法，按照传入的separator字符进行分割。
	 * 需要注意的是：如果,最后一个字符是separator，则说明separator后还有一个空字符串元素
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2013-4-1 下午1:47:49
	 * @param data
	 * @param separator
	 * @return
	 */
	public static String[] splitForCsv(String data, String separator) 
	{
		if(separator.length() > 1)
			return null;
		char [] dataForChar = data.toCharArray();
		char separ = separator.charAt(0); 
	    StringBuffer tempValue = new StringBuffer();
	    int separSum = 0;
	    
	    for (int i = 0; i < dataForChar.length; i++) {
			if(dataForChar[i] == separ)
				separSum ++;
		}
	    String [] totle = new String [separSum + 1];
	    int totleIndex = 0;
	    for (int i = 0; i < dataForChar.length; i++) 
	    {
	    	char c = dataForChar[i];
	    	if(c == separ || i == dataForChar.length - 1)
	    	{
	    		if(i == dataForChar.length - 1)
	    			tempValue.append(c);
	    		totle[totleIndex ++] = tempValue.toString();
	    		tempValue = new StringBuffer();
	    	} else
	    	{
	    		tempValue.append(c);
	    	}
		}
	    
	    for (; totleIndex < totle.length; totleIndex++) 
	    {
	    	totle[totleIndex] = "";
		}
	    
	    return totle;
	}
	public static void main(String[] args) {
		String [] totle = splitForCsv("0000142582\t000089584\t1\t20130108000000000172\t100" , "\t");//new String [5];//
		for (int i = 0; i < totle.length; i++) {
			System.out.println("'" + totle[i] + "'");
		}
		System.out.println(totle.length);
		
	}

	private void setFooter(String[] data) {
		this.size = Integer.parseInt(data[4]);
	}

	/**
	 * 获取头部信息
	 * 
	 * @file: LayerBean.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-28
	 * @param data
	 */
	private void setHead(String[] data) {
		this.sysName = data[1];
		this.msgCode = data[2];
		this.layCode = data[3];
		this.chgname = data[4];
	}

	public String getSysName() {
		return sysName;
	}

	public void setSysName(String sysName) {
		this.sysName = sysName;
	}

	public String getMsgCode() {
		return msgCode;
	}

	public void setMsgCode(String msgCode) {
		this.msgCode = msgCode;
	}

	public String getLayCode() {
		return layCode;
	}

	public void setLayCode(String layCode) {
		this.layCode = layCode;
	}

	public String getChgname() {
		return chgname;
	}

	public void setChgname(String chgname) {
		this.chgname = chgname;
	}

	/**
	 * 生成没一层数据
	 * 
	 * @file: LayerBean.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-28
	 * @param separator
	 *            分隔符
	 * @param ln
	 *            换行符
	 * @return
	 */
	public String toCsv(String separator, String ln) {
		StringBuilder sb = new StringBuilder();
		sb.append(BOF);
		sb.append(separator);
		sb.append(sysName);// 来源系统
		sb.append(separator);
		sb.append(msgCode);// 电文代码
		sb.append(separator);
		sb.append(layCode);// 层代码
		sb.append(separator);
		sb.append(chgname);
		sb.append(ln);
		for (String[] data : dataList) {
			// 添加一条数据
			sb.append(DTA);
			sb.append(separator);
			String dataLine = "";
			for (int i = 0; i < data.length; i++) {
				// 写一列
				dataLine += data[i];
				if(i < data.length -1){
					dataLine += separator;
				}
				// sb.append(str);
				// sb.append(separator);
			}

			sb.append(dataLine);
			// 一行数据写完，换行
			sb.append(ln);
		}
		// 结束行
		sb.append(EOF);
		sb.append(separator);
		sb.append(sysName);// 来源系统
		sb.append(separator);
		sb.append(msgCode);// 电文代码
		sb.append(separator);
		sb.append(layCode);// 层代码
		sb.append(separator);
		if (dataSize > 0)
			sb.append(dataSize);// 数量
		else
			sb.append(dataList.size());// 数量
		// 加个换行符
		sb.append(ln);
		return sb.toString();
	}

	public List<String[]> getDataList() {
		return dataList;
	}

	public void setDataList(List<String[]> dataList) {
		this.dataList = dataList;
	}

	public int getDataSize() {
		return dataSize;
	}

	public void setDataSize(int dataSize) {
		this.dataSize = dataSize;
	}

	public String getChgnamePrefix() {
		chgname = chgname.toUpperCase();
		int index = chgname.indexOf(".CSV");
		return this.chgname.substring(0, index);
	}
}
