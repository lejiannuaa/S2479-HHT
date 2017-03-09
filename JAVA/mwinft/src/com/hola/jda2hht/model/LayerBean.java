package com.hola.jda2hht.model;

import java.io.Serializable;
import java.util.LinkedList;
import java.util.List;

/**
 * 一个层的实体对象
 * 
 * @author 唐植超(上海软通)
 * @date 2012-12-27
 */
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

	public List<String[]> getDataList() {
		return dataList;
	}

	public void setDataList(List<String[]> dataList) {
		this.dataList = dataList;
	}

	// 条数
	private int size;

	public int getSize() {
		return size;
	}

	public void setSize(int size) {
		this.size = size;
	}

	public LayerBean() {
	}

	public LayerBean(String[] layData, String separator) {
		this.setLayer(layData, separator);
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
				data = data.substring(3);
				// data = data.trim();
				this.dataList.add(data.split(separator));
				continue;
			}
			if (data.startsWith(EOF + separator)) {
				setFooter(data.split(separator));
				break;
			}
		}
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

	// /**
	// * 生成没一层数据
	// *
	// * @file: LayerBean.java
	// * @author 唐植超(上海软通)
	// * @date 2012-12-28
	// * @param separator
	// * 分隔符
	// * @param ln
	// * 换行符
	// * @return
	// */
	// public String toCsv(String separator, String ln) {
	// StringBuilder sb = new StringBuilder();
	// sb.append(BOF);
	// sb.append(separator);
	// sb.append(sysName);// 来源系统
	// sb.append(separator);
	// sb.append(msgCode);// 电文代码
	// sb.append(separator);
	// sb.append(layCode);// 层代码
	// sb.append(separator);
	// sb.append(chgname);
	// sb.append(ln);
	// for (String[] data : dataList) {
	// // 添加一条数据
	// sb.append(DTA);
	// sb.append(separator);
	//
	// for (String str : data) {
	// // 写一列
	// sb.append(str);
	// sb.append(separator);
	// }
	// // 一行数据写完，换行
	// sb.append(ln);
	// }
	// // 结束行
	// sb.append(EOF);
	// sb.append(separator);
	// sb.append(sysName);// 来源系统
	// sb.append(separator);
	// sb.append(msgCode);// 电文代码
	// sb.append(separator);
	// sb.append(layCode);// 层代码
	// sb.append(separator);
	// sb.append(dataList.size());// 数量
	// // 加个换行符
	// sb.append(ln);
	// return sb.toString();
	// }
}
