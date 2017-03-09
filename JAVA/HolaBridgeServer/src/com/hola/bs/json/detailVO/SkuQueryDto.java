package com.hola.bs.json.detailVO;

public class SkuQueryDto {
	
	private String sku;
	private String sto;
	private String stk_order_qty;
	private String sku_dsc;
	
	public String getSku() {
		return sku;
	}
	public void setSku(String sku) {
		this.sku = sku;
	}


	public String getSku_dsc() {
		return sku_dsc;
	}
	public void setSku_dsc(String sku_dsc) {
		this.sku_dsc = sku_dsc;
	}
	public String getSto() {
		return sto;
	}
	public void setSto(String sto) {
		this.sto = sto;
	}
	public String getStk_order_qty() {
		return stk_order_qty;
	}
	public void setStk_order_qty(String stk_order_qty) {
		this.stk_order_qty = stk_order_qty;
	}

}
