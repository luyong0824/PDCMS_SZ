webpackJsonp([5], {
	0: function(e, t, n) {
		n(1), e.exports = n(322)
	},
	322: function(e, t, n) {
		"use strict";

		function o(e) {
			return e && e.__esModule ? e : {
			default:
				e
			}
		}
		function a() {
			x ? i(b, x) : geolocation.getCurrentPosition(function(e) {
				this.getStatus() == BMAP_STATUS_SUCCESS ? (b = e.point.lng, x = e.point.lat, i(b, x)) : alert("定位失败，请您开启GPS！")
			}, {
				enableHighAccuracy: !0
			})
		}
		function i(e, t, n) {
			var o, a;
			l.
		default.ajax({
				url: "http://api.map.baidu.com/geoconv/v1/?coords=" + e + "," + t + "&from=1&to=5&ak=myGpMwuxu7Lf3IjBrYGiAuPwC3fzjoZF",
				type: "get",
				async: !0,
				dataType: "jsonp",
				success: function(n, i) {
					var r = new BMap.Point(n.result[0].x, n.result[0].y);
					o = 2 * e - r.lng, a = 2 * t - r.lat, o = o.toFixed(5), a = a.toFixed(5), l.
				default.ajax({
						url: v ? p.config.url + "BaseStationBM/SavePlanningPositionMobile" : p.config.url + "BaseData/SavePlacePositionMobile",
						type: "post",
						dataType: "json",
						data: {
							data: JSON.stringify({
								Id: B,
								Lng: o,
								Lat: a,
								pageSize: 50
							})
						},
						beforeSend: function() {
							f.
						default.show()
						},
						success: function(e) {
							if (console.log(e), "-2" == e.Code && p.helpers.jump2login(), "0" === e.Code) {
								alert(e.Message);
								var t = window.location.href.split("?")[0] + "?id=" + B + "&lng=" + o + "&lat=" + a + "&sn=" + A;
								window.location.replace(t)
							}
						},
						fail: function(e) {
							alert(JSON.stringify(e))
						},
						complete: function(e) {
							f.
						default.hide()
						}
					})
				},
				error: function(e, t, n) {}
			})
		}
		n(299), n(301), n(323);
		var r = n(305),
			l = o(r),
			c = n(324),
			s = o(c),
			p = n(310),
			u = n(325),
			d = o(u),
			g = n(306),
			f = o(g),
			S = n(312),
			M = o(S);
		s.
	default.attach(document.body);
		var h = n(327),
			w = p.helpers.getSearchString("lng"),
			P = p.helpers.getSearchString("lat"),
			B = p.helpers.getSearchString("id"),
			A = p.helpers.getSearchString("sn"),
			v = p.helpers.getSearchString("type"),
			y = (window.location.href, new d.
		default ({
				content: "您将更新站点的经纬度，确认吗？",
				btnList: [{
					text: "确认",
					click: function() {
						a()
					}
				}, {
					text: "取消",
					click: function() {
						console.log("取消")
					}
				}]
			})),
			m = window.navigator.userAgent.toLocaleLowerCase(),
			C = {},
			b = "",
			x = "";
		C = {
			android: !! m.match(/Android/i),
			ios: !! m.match(/(?:iPhone|iPad)/i)
		}, new M.
	default ({
			middle: {
				text: "方位更新",
				click: function() {
					y.show()
				}
			},
			right: {
				text: "导航",
				click: function() {
					x ? p.helpers.openApp(x, b, P, w, !0) : geolocation.getCurrentPosition(function(e) {
						this.getStatus() == BMAP_STATUS_SUCCESS ? (b = e.point.lng, x = e.point.lat, p.helpers.openApp(x, b, P, w, !0)) : alert("定位失败，请您开启GPS!")
					}, {
						enableHighAccuracy: !0
					})
				}
			}
		});
		var _ = new BMap.Map("allmap");
		_.addControl(new BMap.NavigationControl), _.addControl(new BMap.MapTypeControl({
			mapTypes: [BMAP_NORMAL_MAP, BMAP_HYBRID_MAP, BMAP_SATELLITE_MAP]
		})), _.enableScrollWheelZoom(), _.enableContinuousZoom(), _.enableInertialDragging();
		var T = "http://api.map.baidu.com/geoconv/v1/?coords=" + w + "," + P + "&from=1&to=5&ak=myGpMwuxu7Lf3IjBrYGiAuPwC3fzjoZF";
		window.location.origin + "/PDCMSWeb";
		l.
	default.ajax({
			url: T,
			type: "get",
			async: !0,
			dataType: "jsonp",
			success: function(e, t) {
				var n = new BMap.Point(e.result[0].x, e.result[0].y),
					o = [n],
					a = new BMap.Icon(h, new BMap.Size(50, 50)),
					i = new BMap.Geolocation,
					r = void 0,
					l = void 0;
				_.enableScrollWheelZoom(!0), i.getCurrentPosition(function(e) {
					if (this.getStatus() == BMAP_STATUS_SUCCESS) {
						var t;
						b = e.point.lng, x = e.point.lat, t = new BMap.Point(b, x), o.push(t), l = new BMap.Marker(t, {
							icon: a
						}), r = _.getViewport(o), _.centerAndZoom(r.center, r.zoom);
						var i = new BMap.Marker(n);
						_.addOverlay(i);
						var c = new BMap.Label(A, {
							offset: new BMap.Size(20, -10)
						});
						i.setLabel(c), l && _.addOverlay(l)
					} else alert("定位失败，请您开启GPS！")
				}, {
					enableHighAccuracy: !0
				})
			},
			error: function(e, t, n) {}
		})
	}
});