package com.hola.common.model.mw;

import java.util.List;

import com.hola.common.model.BaseModel;

public class Chginfo extends BaseModel 
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	
	private		String		chgcode;
	private		String		msgcode;
	private		String		iotype;
	private		String		srccode;
	private		String		taroptyp;//目标档资料操作类型(0:insert; 1:先delete再insert; 2:update)
	private		String		seqcode;
	
	private		List<ChgTbl>	chgtblList;
	
	public String getChgcode() {
		return chgcode;
	}
	public void setChgcode(String chgcode) {
		this.chgcode = chgcode;
	}
	public String getMsgcode() {
		return msgcode;
	}
	public void setMsgcode(String msgcode) {
		this.msgcode = msgcode;
	}
	public String getIotype() {
		return iotype;
	}
	public void setIotype(String iotype) {
		this.iotype = iotype;
	}
	public String getSrccode() {
		return srccode;
	}
	public void setSrccode(String srccode) {
		this.srccode = srccode;
	}
	public List<ChgTbl> getChgtblList() {
		return chgtblList;
	}
	public void setChgtblList(List<ChgTbl> chgtblList) {
		this.chgtblList = chgtblList;
	}
	public String getTaroptyp() {
		return taroptyp;
	}
	public void setTaroptyp(String taroptyp) {
		this.taroptyp = taroptyp;
	}
	public String getSeqcode() {
		return seqcode;
	}
	public void setSeqcode(String seqcode) {
		this.seqcode = seqcode;
	}
}
