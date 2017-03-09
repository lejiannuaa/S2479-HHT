package com.hola.common.model.mw;

import com.hola.common.model.BaseModel;

public class ChgTblDtl extends BaseModel 
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	
	private		String		id;
	private		String		tblId;
	private		String		srcLayCode;
	private		String		srcFldCode;
	private		Integer		srcSeq;
	private		String		tarLayCode;
	private		String		tarFldCode;
	private		Integer		tarSeq;
	private		String		isPk;
	private		String		defaultValue;
	
	public String getId() {
		return id;
	}
	public void setId(String id) {
		this.id = id;
	}
	public String getTblId() {
		return tblId;
	}
	public void setTblId(String tblId) {
		this.tblId = tblId;
	}
	public String getSrcLayCode() {
		return srcLayCode;
	}
	public void setSrcLayCode(String srcLayCode) {
		this.srcLayCode = srcLayCode;
	}
	public String getSrcFldCode() {
		return srcFldCode;
	}
	public void setSrcFldCode(String srcFldCode) {
		this.srcFldCode = srcFldCode;
	}
	public String getTarLayCode() {
		return tarLayCode;
	}
	public void setTarLayCode(String tarLayCode) {
		this.tarLayCode = tarLayCode;
	}
	public String getTarFldCode() {
		return tarFldCode;
	}
	public void setTarFldCode(String tarFldCode) {
		this.tarFldCode = tarFldCode;
	}
	public String getIsPk() {
		return isPk;
	}
	public void setIsPk(String isPk) {
		this.isPk = isPk;
	}
	public Integer getSrcSeq() {
		return srcSeq;
	}
	public void setSrcSeq(Integer srcSeq) {
		this.srcSeq = srcSeq;
	}
	public Integer getTarSeq() {
		return tarSeq;
	}
	public void setTarSeq(Integer tarSeq) {
		this.tarSeq = tarSeq;
	}
	public String getDefaultValue() {
		return defaultValue;
	}
	public void setDefaultValue(String defaultValue) {
		this.defaultValue = defaultValue;
	}
}
