webpackJsonp([15], {
	0: function(e, t, a) {
		a(1), e.exports = a(349)
	},
	349: function(e, t, a) {
		"use strict";

		function l(e) {
			return e && e.__esModule ? e : {
			default:
				e
			}
		}
		function i() {
			(0, c.
		default)("#name").val(m)
		}
		function n() {
			var e = (0, c.
		default)(".img-list").find("ul"),
				t = e.eq(0),
				a = e.eq(1),
				l = e.eq(2),
				i = t.height(),
				n = a.height(),
				d = l.height();
			return i <= n ? i <= d ? t : l : n <= d ? a : l
		}
		function d() {
			c.
		default.ajax({
				url: h.config.url + "BaseStationBM/GetProjectProgressByIdMobile",
				type: "post",
				dataType: "json",
				data: {
					id: j,
					TypeId: 1,
					pageSize: 50
				},
				beforeSend: function() {
					g.
				default.show()
				},
				success: function(e) {
					if (console.log(e), "-2" == e.Code) return h.helpers.jump2login("../views/xmjddj.html?id=" + j + "&pn=" + m), !1;
					w = e.ProjectProgress;
					var t = e.ImageUrl,
						a = e.ProgressMemos || "";
					M[w || "0"].selected = 1, (0, c.
				default)("#jd").val(M[w || "0"].text), (0, c.
				default)("#jdjs").text(a), P = new r.
				default ({
						dataList: M,
						cb: function(e, t, a) {
							w = t, (0, c.
						default)("#jd").val(e)
						}
					});
					var l = t.split(",");
					l.length > 0 && l.forEach(function(e, t) {
						if (3 != (0, c.
					default)(".img-wrap").find("ul").length)(0, c.
					default)(".img-wrap").append("<ul><li><img src=" + e + ' width="100%;" /><!--<span></span>--></li></ul>');
						else {
							var a = n();
							a.append("<li><img src=" + e + ' width="100%;" /><!--<span></span>--></li>')
						}
					}), (0, c.
				default)("#jdjs").val(a)
				},
				fail: function(e) {},
				complete: function(e) {
					g.
				default.hide()
				}
			})
		}
		a(299), a(301), a(333);
		var u = a(312),
			o = l(u),
			s = a(330),
			r = l(s),
			f = a(309),
			c = l(f),
			p = a(306),
			g = l(p),
			h = a(310),
			v = a(334);
		l(v);
		a(335), new o.
	default ({
			middle: {
				text: "项目进度登记"
			}
		});
		var m = h.helpers.getSearchString("pn"),
			j = h.helpers.getSearchString("id"),
			w = "",
			x = "",
			S = void 0,
			y = "",
			M = [{
				value: "",
				text: "请选择"
			}, {
				value: "1",
				text: "未开工"
			}, {
				value: "2",
				text: "进行中"
			}, {
				value: "3",
				text: "已完工"
			}, {
				value: "4",
				text: "暂缓"
			}, {
				value: "5",
				text: "取消"
			}],
			P = void 0;
		i(), (0, c.
	default)("#save").on("click", function() {
			return j ? w ? (x = c.
		default.trim((0, c.
		default)("#jdjs").val()), void c.
		default.ajax({
				url: h.config.url + "BaseStationBM/SaveProgressMobile",
				type: "post",
				dataType: "json",
				data: {
					data: JSON.stringify({
						Id: j,
						Base64String: y ? JSON.stringify(y) : "",
						ProjectProgress: w,
						ProgressMemos: x,
						pageSize: 50
					}),
					TypeId: 1
				},
				beforeSend: function() {
					g.
				default.show()
				},
				success: function(e) {
					return "-2" == e.Code ? (h.helpers.jump2login("../views/xmjddj.html?id=" + j + "&pn=" + m), !1) : void("0" === e.Code ? (alert(e.Message), window.location.href = "./jddjlist.html?type=1") : alert("保存失败，请重试"))
				},
				fail: function(e) {},
				complete: function(e) {
					g.
				default.hide()
				}
			})) : (alert("请选择项目进度"), !1) : (alert("ID不能为空！"), !1)
		}), (0, c.
	default)(".select").on("click", function() {
			P.show()
		}), (0, c.
	default)("#imguploader").on("click", function() {
			(0, c.
		default)("#uploadphoto").click()
		}), (0, c.
	default)("#uploadphoto").localResizeIMG({
			width: 400,
			quality: .1,
			success: function(e) {
				S = e.base64;
				n();
				if (y || (y = []), y.push(S), 3 != (0, c.
			default)(".img-wrap").find("ul").length)(0, c.
			default)(".img-wrap").append("<ul><li><img src=" + S + ' width="100%;" /><!--<span></span>--></li></ul>');
				else {
					var t = n();
					t.append("<li><img src=" + S + ' width="100%;" /><!--<span></span>--></li>')
				}
			}
		}), d()
	}
});