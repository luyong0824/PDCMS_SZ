webpackJsonp([10], {
	0: function(e, a, l) {
		l(1), e.exports = l(340)
	},
	340: function(e, a, l) {
		(function(e) {
			"use strict";

			function a(e) {
				return e && e.__esModule ? e : {
				default:
					e
				}
			}
			l(299), l(301), l(341);
			var s = l(312),
				i = a(s),
				d = l(310),
				c = l(306),
				v = a(c),
				t = d.helpers.getSearchString("id"),
				n = '<div class="content"><div class="item"><label for="">站点名称:</label><div class="message">{PlaceName}</div></div><div class="item"><label for="">站点类型:</label><div class="message">{PlaceCategoryName}</div></div><div class="item"><label for="">区&nbsp;&nbsp;域:</label><div class="message">{AreaName}</div></div><div class="item"><label for="">网&nbsp;&nbsp;格:</label><div class="message">{ReseauName}</div></div><div class="item"><label for="">产&nbsp;&nbsp;权:</label><div class="message">{PlaceOwnerName}</div></div><div class="item"><label for="">导航图:</label><div class="message map"><a href="fwgx.html?id={Id}&lng={Lng}&lat={Lat}&sn={PlaceName}">地图</a></div></div><div class="item"><label for="">业主名称:</label><div class="message">{OwnerName}</div></div><div class="item"><label for="">联系人:</label><div class="message">{OwnerContact}</div></div><div class="item"><label for="">联系方式:</label><div class="message">{OwnerPhoneNumber}</div></div><div class="item"><label for="">2G逻辑号:</label><div class="message">{G2Number}</div></div><div class="item"><label for="">3G逻辑号:</label><div class="message">{G3Number}</div></div><div class="item"><label for="">4G逻辑号:</label><div class="message">{G4Number}</div></div></div>';
			e.ajax({
				url: d.config.url + "BaseData/GetPlaceInfoMobile/" + t,
				dataType: "json",
				data: {
					pageSize: 50
				},
				beforeSend: function() {
					v.
				default.show()
				},
				success: function(a) {
					console.log(a), "-2" == a.Code && d.helpers.jump2login(), "0" === a.Code, new i.
				default ({
						middle: {
							text: a.PlaceName
						}
					}), e("#data").html(d.helpers.template(n, [a]))
				},
				fail: function(e) {},
				complete: function(e) {
					v.
				default.hide()
				}
			})
		}).call(a, l(309))
	},
	341: function(e, a) {}
});