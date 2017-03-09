package com.hola.jda2hht.model;

import java.io.Serializable;

/**
 * 交换资料说明表的明细
 * 
 * @author 唐植超(上海软通)
 * @date 2012-12-26
 */
public class ChangeTableDetailBean implements Serializable {

	private static final long serialVersionUID = 1L;
	// 编号
	private String id;
	// 表说明编号
	private String tblid;
	// 源层代码
	private String srclaycode;
	// 源栏位代码
	private String srcfldcode;
	// 源栏位序列
	private String srcseq;
	// 目标层代码
	private String tarlaycode;
	// 目标栏位代码
	private String tarfldcode;
	// 目标栏位序列
	private String tarseq;
	// 是否是门店号
	private String isstr;
	// 是否需要是排序的列
	private String isseq;
	
	//是否转换到SOM到JDA 状态为I
	private String tochange;

	public String getIsseq() {
		return isseq;
	}

	public void setIsseq(String isseq) {
		this.isseq = isseq;
	}

	public String getIsstr() {
		return isstr;
	}

	public void setIsstr(String isstr) {
		this.isstr = isstr;
	}

	public String getId() {
		return id;
	}

	public void setId(String id) {
		this.id = id;
	}

	public String getTblid() {
		return tblid;
	}

	public void setTblid(String tblid) {
		this.tblid = tblid;
	}

	public String getSrclaycode() {
		return srclaycode;
	}

	public void setSrclaycode(String srclaycode) {
		this.srclaycode = srclaycode;
	}

	public String getSrcfldcode() {
		return srcfldcode;
	}

	public void setSrcfldcode(String srcfldcode) {
		this.srcfldcode = srcfldcode;
	}

	public String getSrcseq() {
		return srcseq;
	}

	public void setSrcseq(String srcseq) {
		this.srcseq = srcseq;
	}

	public String getTarlaycode() {
		return tarlaycode;
	}

	public void setTarlaycode(String tarlaycode) {
		this.tarlaycode = tarlaycode;
	}

	public String getTarfldcode() {
		return tarfldcode;
	}

	public void setTarfldcode(String tarfldcode) {
		this.tarfldcode = tarfldcode;
	}

	public String getTarseq() {
		return tarseq;
	}

	public void setTarseq(String tarseq) {
		this.tarseq = tarseq;
	}

	public String getTochange() {
		return tochange;
	}

	public void setTochange(String tochange) {
		this.tochange = tochange;
	}

}
