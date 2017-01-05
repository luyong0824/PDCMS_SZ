webpackJsonp([13], {
	0: function(e, t, a) {
		a(1), e.exports = a(347)
	},
	347: function(e, t, a) {
		"use strict";

		function o(e) {
			return e && e.__esModule ? e : {
			default:
				e
			}
		}
		function n(e, t, a) {
			var o, n;
			d.
		default.ajax({
				url: "http://api.map.baidu.com/geoconv/v1/?coords=" + e + "," + t + "&from=1&to=5&ak=myGpMwuxu7Lf3IjBrYGiAuPwC3fzjoZF",
				type: "get",
				async: !0,
				dataType: "jsonp",
				success: function(l, i) {
					var u = new BMap.Point(l.result[0].x, l.result[0].y),
						u = new BMap.Point(l.result[0].x, l.result[0].y);
					o = 2 * e - u.lng, n = 2 * t - u.lat, o = o.toFixed(5), n = n.toFixed(5), a(o, n)
				},
				error: function(e, t, a) {}
			})
		}
		function l(e) {
			alert(e)
		}
		function i() {
			d.
		default.ajax({
				url: f.config.url + "BaseData/GetAllAreas",
				type: "post",
				dataType: "json",
				data: {
					pageSize: 50
				},
				beforeSend: function() {
					r.
				default.show()
				},
				success: function(e) {
					if (e.Code && "-2" == e.Code) return void f.helpers.jump2login(window.location.href);
					w.data = e;
					var t = (0, p.
				default)("layoutTemp", w);
					document.getElementById("layout").innerHTML = t
				},
				fail: function(e) {
					console.log("登录失败,请重试")
				},
				complete: function(e) {
					r.
				default.hide()
				}
			})
		}
		a(299), a(301), a(302);
		var u = a(309),
			d = o(u),
			c = a(306),
			r = o(c),
			f = a(310),
			s = a(311),
			p = o(s),
			g = a(312),
			h = o(g),
			v = a(334);
		o(v);
		a(335), new h.
	default ({
			middle: {
				text: "盲点反馈"
			}
		});
		var m = "",
			y = "",
			B = f.helpers.getSearchString("curLng"),
			x = f.helpers.getSearchString("curLat");
		n(B, x, function(e, t) {
			(0, d.
		default)("#saveBtn").on("click", function() {
				var a = (0, d.
			default)("#addressName").val(),
					o = (0, d.
				default)("#describe").val(),
					n = [];
				if ("" == m) return void l("请输入区域！");
				if ("" == a) return void l("请输入盲点地名！");
				if ("" == o) return void l("请输入简述！");
				if ((0, d.
			default)("#photoBox").find("img").length > 0) for (var i = 0, u = (0, d.
			default)("#photoBox").find("img").length; i < u; i++) n.push((0, d.
			default)("#photoBox").find("img")[i].src);
				var c = JSON.stringify({
					Id: "00000000-0000-0000-0000-000000000000",
					AreaId: m,
					PlaceName: a,
					Lng: e,
					Lat: t,
					FeedBackContent: o,
					Base64String: 0 == n.length ? "" : JSON.stringify(n),
					pageSize: 50
				});
				d.
			default.ajax({
					url: f.config.url + "BaseStationBM/SaveBlindSpotFeedBackMobile",
					type: "post",
					dataType: "json",
					data: {
						data: c
					},
					beforeSend: function() {
						r.
					default.show()
					},
					success: function(e) {
						return e.Code && "-2" == e.Cod ? void f.helpers.jump2login(window.location.href) : void alert("保存成功！")
					},
					fail: function(e) {
						console.log("登录失败,请重试")
					},
					complete: function(e) {
						r.
					default.hide()
					}
				})
			})
		}), (0, d.
	default)("#selectArea").on("click", function() {
			(0, d.
		default)("#showLayout").addClass("active")
		}), (0, d.
	default)("#layMarker,#cancelBtn").on("click", function() {
			(0, d.
		default)("#showLayout").removeClass("active")
		}), (0, d.
	default)("#layout").delegate("li", "click", function(e) {
			var t = ((0, d.
		default)(e.target.parentNode).attr("data-type"), (0, d.
		default)(this).attr("data-id")),
				a = (0, d.
			default)(this).text();
			m = t, (0, d.
		default)("#selectArea").text(a), (0, d.
		default)("#showLayout").removeClass("active")
		});
		var w = {};
		i(), (0, d.
	default)("#cameraIcon").on("click", function() {
			var e = (0, d.
		default)("#photoBox").find("img").length;
			return e > 1 ? void alert("最多上传2两张图片！") : void(0, d.
		default)("#uploadPhoto").click()
		}), (0, d.
	default)("#uploadPhoto").localResizeIMG({
			width: 400,
			quality: 1,
			success: function(e) {
				y = e.base64;
				var t = "<span><img src='" + y + "'/><em class='cancel'></em></span>";
				(0, d.
			default)("#photoBox").append(t)
			}
		}), (0, d.
	default)("#photoBox").delegate("em", "click", function() {
			(0, d.
		default)(this).parent("span").remove()
		})
	}
});