package com.hola.bs.bean;

/**
 * 指令信息的记录对象
 * 
 * @author S2139 2012 Oct 24, 2012 4:27:04 PM
 */
/**
 * 
 * @author S2139
 * 2012 Nov 1, 2012 11:04:04 AM 
 */
public class CommandContainer {

	/**
	 * 该指令的总长度
	 */
	private int totalSize;

	/**
	 * 当前缓存中存放的指令信息内容
	 */
	private StringBuffer currBuffer;

	/**
	 * 当前总共接收到的总长度
	 */
	private int currSize;

	/**
	 * 该指令的KEY值
	 */
	private String guid;

	
	public CommandContainer() {

	}

	public CommandContainer(int totalSize, StringBuffer currBuffer,
			int currSize, String guid) {
		this.totalSize = totalSize;
		this.currBuffer = currBuffer;
		this.currSize = currSize;
		this.guid = guid;
	}

	public int getTotalSize() {
		return totalSize;
	}

	public void setTotalSize(int totalSize) {
		this.totalSize = totalSize;
	}

	public StringBuffer getCurrBuffer() {
		return currBuffer;
	}

	public void setCurrBuffer(StringBuffer currBuffer) {
		this.currBuffer = currBuffer;
	}

	public int getCurrSize() {
		return currSize;
	}

	public void setCurrSize(int currSize) {
		this.currSize = currSize;
	}

	public String getGuid() {
		return guid;
	}

	public void setGuid(String guid) {
		this.guid = guid;
	}


}
