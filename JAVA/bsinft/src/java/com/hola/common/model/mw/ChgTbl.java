package com.hola.common.model.mw;

import java.util.ArrayList;
import java.util.List;

import com.hola.common.model.BaseModel;

public class ChgTbl extends BaseModel 
{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	
	private		String		id;
	private		String		chgcode;
	private		String		srcTableName;
	private		String		tarTableName;
	private		String		tarOpcond;
	private		String		srcOpcond;
	
	private		Integer				layNo;
	private		List<ChgTblDtl>		cols;
	private		String				colStr;
	private		String				pkColStr;
	private		Integer				instnoColNo;
	private		final	List<Integer>		pkColNo = 
				new ArrayList<Integer>();
	
	public String getChgcode() {
		return chgcode;
	}
	public void setChgcode(String chgcode) {
		this.chgcode = chgcode;
	}
	public String getSrcTableName() {
		return srcTableName;
	}
	public void setSrcTableName(String srcTableName) {
		this.srcTableName = srcTableName;
	}
	public String getTarTableName() {
		return tarTableName;
	}
	public void setTarTableName(String tarTableName) {
		this.tarTableName = tarTableName;
	}
	public String getTarOpcond() {
		return tarOpcond;
	}
	public void setTarOpcond(String tarOpcond) {
		this.tarOpcond = tarOpcond;
	}
	public String getId() {
		return id;
	}
	public void setId(String id) {
		this.id = id;
	}
	public int getLayNo() {
		return layNo;
	}
	public void setLayNo(int layNo) {
		this.layNo = layNo;
	}
	public List<ChgTblDtl> getCols() {
		return cols;
	}
	public void setCols(List<ChgTblDtl> cols) {
		this.cols = cols;
	}
	public String getColStr() {
		return colStr;
	}
	public void setColStr(String colStr) {
		this.colStr = colStr;
	}
	public String getPkColStr() {
		return pkColStr;
	}
	public void setPkColStr(String pkColStr) {
		this.pkColStr = pkColStr;
	}
	public List<Integer> getPkColNo() {
		return pkColNo;
	}
	public Integer getInstnoColNo() {
		return instnoColNo;
	}
	public void setInstnoColNo(Integer instnoColNo) {
		this.instnoColNo = instnoColNo;
	}
	public String getSrcOpcond() {
		return srcOpcond;
	}
	public void setSrcOpcond(String srcOpcond) {
		this.srcOpcond = srcOpcond;
	}
}
