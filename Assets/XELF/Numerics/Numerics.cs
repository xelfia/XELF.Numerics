// © 2018 XELF
// XELF.Numerics
// https://github.com/xelfia/XELF.Numerics

using UnityEngine;
using static UnityEngine.Mathf;

namespace XELF.Experimental.Numerics {
	/// <summary>
	/// Real: w
	/// </summary>
	public readonly struct R {
		public readonly float w;
		public override string ToString() => w.ToString();
		public R(float w) => this.w = w;
		public static implicit operator R(float x) => new R(x);
		public static R operator -(R r) => -r.w;
		public static (R x, R y) operator *(R s, (R x, R y) t)
			=> (s.w * t.x.w, s.w * t.y.w);
		public static R operator +(R a, R b) => a.w + b.w;
		public static R operator -(R a, R b) => a.w - b.w;
		public static R operator *(R a, R b) => a.w * b.w;
		public static R operator /(R a, R b) => a.w / b.w;
		public static R Dot(in (I i, J j, K k) a, in (I i, J j, K k) b) => a.i * b.i + a.j * b.j + a.k * b.k;
		public static (R r, I j) operator *(R s, (R r, I i) b) => (s.w * b.r.w, s.w * b.i);
		public static (I i, J j, K k) operator *(R r, in (I i, J j, K k) b) => (r * b.i, r * b.j, r * b.k);
		public static (D x, D y) operator *(R s, (D x, D y) t)
			=> (s.w * t.x.d, s.w * t.y.d);
		public static (R r, D d) operator *(R s, (R r, D d) b) => (s.w * b.r.w, s.w * b.d.d);
	}

	public readonly struct Reciprocal {
		public readonly float x;
		public Reciprocal(float x) => this.x = x;
		public override string ToString() => $"({x})⁻¹";
		public static Reciprocal operator *(Reciprocal a, Reciprocal b) => new Reciprocal(a.x * b.x);
		public static H<R> operator *(Reciprocal a, R b) => new H<R>(a, b);
		public static H<R> operator *(R a, Reciprocal b) => new H<R>(b, a);
	}
	public readonly struct H<T> {
		public readonly T x;
		public readonly Reciprocal w;
		public override string ToString() => $"({x})/{w.x}";
		public H(Reciprocal r, T t) => (x, w) = (t, r);
		public H(T t, R w) => (x, this.w) = (t, new Reciprocal(w.w));
	}
	public readonly struct HR {
		public readonly H<R> x;
		public static implicit operator HR(H<R> x) => new HR(x);
		public override string ToString() => x.ToString();
		public HR(H<R> x) => this.x = x;
		public static HR operator *(HR a, HR b) => new H<R>(a.x.w * b.x.w, a.x.x * b.x.x);
		public static HR operator /(HR a, HR b) => new H<R>(a.x.w * new Reciprocal(b.x.x.w), a.x.x * b.x.w.x);
		public static explicit operator R(HR x) => x.x.x.w / x.x.w.x;
		public static explicit operator float(HR x) => x.x.x.w / x.x.w.x;
	}
	public readonly struct HRIJK {
		public readonly H<RIJK> x;
		public override string ToString() => x.ToString();
		public HRIJK(H<RIJK> x) => this.x = x;
		public HRIJK(Reciprocal a, RIJK b) => x = new H<RIJK>(a, b);
		public static implicit operator HRIJK((RIJK x, Reciprocal r) t) => new HRIJK(new H<RIJK>(t.r, t.x));
		public static explicit operator HRIJK(H<RIJK> x) => new HRIJK(x);
		public static explicit operator RIJK(HRIJK x) => (1 / x.x.w.x) * x.x.x;
		public static HRIJK operator *(HRIJK x, HRIJK y) => (x.x.x * y.x.x, x.x.w * y.x.w);
	}

	/// <summary>
	/// Dual Unit ε: dε
	/// </summary>
	public readonly struct D {
		public readonly float d;
		public override string ToString() => d.ToString() + "ε";
		public D(float d) => this.d = d;
		public static D operator -(D i) => new D(-i.d);
		public static implicit operator D(float d) => new D(d);
		public static R operator *(D a, D b) => 0;
		public static D operator *(R w, D d) => w.w * d.d;
		public static D operator +(D a, D b) => a.d + b.d;
		public static D operator -(D a, D b) => a.d - b.d;
		public static D operator /(D a, R b) => a.d / b.w;
	}
	/// <summary>
	/// Quaternion Unit I: xi
	/// </summary>
	public readonly struct I {
		public readonly float x;
		public override string ToString() => x.ToString() + "i";
		public I(float x) => this.x = x;
		public static I operator -(I i) => new I(-i.x);
		public static explicit operator I(float i) => new I(i);
		public static I operator +(I a, I b) => (I)(a.x + b.x);
		public static I operator -(I a, I b) => (I)(a.x - b.x);
		public static I operator *(R a, I b) => (I)(a.w * b.x);
		public static R operator *(I a, I b) => -a.x * b.x;
		public static K operator *(I a, J b) => (K)(a.x * b.y);
		public static K operator *(J a, I b) => -(K)(a.y * b.x);
		public static (R r, I i) operator +(R r, I i) => (r, i);
		public static (R r, I i) operator -(R r, I i) => (r, -i);
	}
	/// <summary>
	/// Dual Quaternion Unit I: xεi
	/// </summary>
	public readonly struct DI {
		public readonly float x;
		public override string ToString() => x.ToString() + "εi";
		public DI(float x) => this.x = x;
		public static DI operator -(DI i) => new DI(-i.x);
		public static explicit operator DI(float i) => new DI(i);
		public static DI operator +(DI a, DI b) => (DI)(a.x + b.x);
		public static DI operator -(DI a, DI b) => (DI)(a.x - b.x);
		public static DI operator *(R a, DI b) => (DI)(a.w * b.x);
		public static R operator *(I a, DI b) => -a.x * b.x;
		public static DK operator *(DI a, J b) => (DK)(a.x * b.y);
		public static DK operator *(J a, DI b) => -(DK)(a.y * b.x);
		public static (R r, DI i) operator +(R r, DI i) => (r, i);
		public static (R r, DI i) operator -(R r, DI i) => (r, -i);
	}
	/// <summary>
	/// Quaternion Unit J: yj
	/// </summary>
	public readonly struct J {
		public readonly float y;
		public override string ToString() => y.ToString() + "j";
		public J(float y) => this.y = y;
		public static J operator -(J j) => new J(-j.y);
		public static explicit operator J(float j) => new J(j);
		public static J operator +(J a, J b) => (J)(a.y + b.y);
		public static J operator -(J a, J b) => (J)(a.y - b.y);
		public static J operator *(R a, J b) => (J)(a.w * b.y);
		public static R operator *(J a, J b) => -a.y * b.y;
		public static I operator *(J a, K b) => (I)(a.y * b.z);
		public static I operator *(K a, J b) => -(I)(a.z * b.y);
		public static (R r, J j) operator +(R r, J j) => (r, j);
		public static (R r, J j) operator -(R r, J j) => (r, -j);
	}
	/// <summary>
	/// Quaternion Unit J: yεj
	/// </summary>
	public readonly struct DJ {
		public readonly float y;
		public override string ToString() => y.ToString() + "εj";
		public DJ(float y) => this.y = y;
		public static DJ operator -(DJ j) => new DJ(-j.y);
		public static explicit operator DJ(float j) => new DJ(j);
		public static DJ operator +(DJ a, DJ b) => (DJ)(a.y + b.y);
		public static DJ operator -(DJ a, DJ b) => (DJ)(a.y - b.y);
		public static DJ operator *(R a, DJ b) => (DJ)(a.w * b.y);
		public static R operator *(J a, DJ b) => -a.y * b.y;
		public static DI operator *(DJ a, K b) => (DI)(a.y * b.z);
		public static DI operator *(K a, DJ b) => -(DI)(a.z * b.y);
		public static (R r, DJ j) operator +(R r, DJ j) => (r, j);
		public static (R r, DJ j) operator -(R r, DJ j) => (r, -j);
	}
	/// <summary>
	/// Quaternion Unit K: zk
	/// </summary>
	public readonly struct K {
		public readonly float z;
		public override string ToString() => z.ToString() + "k";
		public K(float z) => this.z = z;
		public static K operator -(K k) => new K(-k.z);
		public static explicit operator K(float k) => new K(k);
		public static K operator +(K a, K b) => (K)(a.z + b.z);
		public static K operator -(K a, K b) => (K)(a.z - b.z);
		public static K operator *(R a, K b) => (K)(a.w * b.z);
		public static R operator *(K a, K b) => -a.z * b.z;
		public static J operator *(I a, K b) => -(J)(a.x * b.z);
		public static J operator *(K a, I b) => (J)(a.z * b.x);
		public static (R r, K k) operator +(R r, K k) => (r, k);
		public static (R r, K k) operator -(R r, K k) => (r, -k);
	}
	/// <summary>
	/// Dual Quaternion Unit K: zεk
	/// </summary>
	public readonly struct DK {
		public readonly float z;
		public override string ToString() => z.ToString() + "εk";
		public DK(float z) => this.z = z;
		public static DK operator -(DK k) => new DK(-k.z);
		public static explicit operator DK(float k) => new DK(k);
		public static DK operator +(DK a, DK b) => (DK)(a.z + b.z);
		public static DK operator -(DK a, DK b) => (DK)(a.z - b.z);
		public static DK operator *(R a, DK b) => (DK)(a.w * b.z);
		public static R operator *(K a, DK b) => -a.z * b.z;
		public static DJ operator *(I a, DK b) => -(DJ)(a.x * b.z);
		public static DJ operator *(DK a, I b) => (DJ)(a.z * b.x);
		public static (R r, DK k) operator +(R r, DK k) => (r, k);
		public static (R r, DK k) operator -(R r, DK k) => (r, -k);
	}
	/// <summary>
	/// w + dε
	/// </summary>
	public readonly struct RD {
		public readonly R r;
		public readonly D d;
		public override string ToString() => $"{r}{d.d:+#..######;-#..######;+0}ε";
		public RD(R r) => (this.r, d) = (r, 0);
		public RD(D d) => (r, this.d) = (0, d);
		public RD(R r, D d) => (this.r, this.d) = (r, d);
		public static implicit operator RD((R r, D d) t) => new RD(t.r, t.d);
		public static RD operator +(RD a, RD b) => (a.r + b.r, a.d + b.d);
		public static RD operator -(RD a, RD b) => (a.r - b.r, a.d - b.d);
		public static RD operator *(RD a, RD b) => (a.r * b.r, a.r * b.d + b.r * a.d);
		public static RD operator /(RD a, RD b) => (a.r / b.r, (b.r * a.d - a.r * b.d) / (b.r * b.r));
	}
	/// <summary>
	/// xi + yj + zk
	/// </summary>
	public readonly struct IJK {
		public readonly I i;
		public readonly J j;
		public readonly K k;
		public override string ToString() => $"{i.x:+0.######;-0.######;0}i{j.y:+0.######;-0.######;+0}j{k.z:+0.######;-0.######;+0}k";
		public IJK(I x, J y, K z) => (i, j, k) = (x, y, z);
		public static implicit operator IJK(in (I i, J j, K k) t) => new IJK(t.i, t.j, t.k);
		public static IJK operator *(R r, IJK b) => (r * b.i, r * b.j, r * b.k);
		public static IJK operator +(IJK a, IJK b) => (a.i + b.i, a.j + b.j, a.k + b.k);
		public static R Dot(in IJK a, in IJK b) => a.i * b.i + a.j * b.j + a.k * b.k;
		public static IJK Cross(in IJK a, in IJK b) => (a.j * b.k + a.k * b.j, a.k * b.i + a.i * b.k, a.i * b.j + a.j * b.i);
	}
	/// <summary>
	/// Quaternion: w + xi + ij + zk
	/// </summary>
	public readonly struct RIJK {
		public readonly R r;
		public readonly I i;
		public readonly J j;
		public readonly K k;
		public IJK ijk => (i, j, k);
		public override string ToString() => $"{r}{i.x:+0.######;-0.######;+0}i{j.y:+0.######;-0.######;+0}j{k.z:+0.######;-0.######;+0}k";
		public static implicit operator RIJK(in (R r, IJK v) t) => new RIJK(t.r, t.v.i, t.v.j, t.v.k);
		public static implicit operator RIJK(in (R r, (I x, J y, K z) t) t) => new RIJK(t.r, t.t.x, t.t.y, t.t.z);
		public static implicit operator RIJK(in (R r, I x, J y, K z) t) => new RIJK(t.r, t.x, t.y, t.z);
		public RIJK(R w, I x, J y, K z) => (r, i, j, k) = (w, x, y, z);
		public RIJK(I x, J y, K z, R w) => (r, i, j, k) = (w, x, y, z);
		public HRIJK Normalized => ReciprocalLength * (RIJK)(r, i, j, k);
		public R LengthSquared => r * r - i * i - j * j - k * k;
		public R Length => Sqrt(LengthSquared.w);
		public Reciprocal ReciprocalLength => new Reciprocal(Length.w);
		public static RIJK operator +(in RIJK a, in RIJK b) => (a.r + b.r, a.i + b.i, a.j + b.j, a.k + b.k);
		public static RIJK operator -(in RIJK a, in RIJK b) => (a.r - b.r, a.i - b.i, a.j - b.j, a.k - b.k);
		public static RIJK operator *(R a, in RIJK b) => (a.w * b.r, a.w * b.i, a.w * b.j, a.w * b.k);
		public static R Dot(in RIJK a, in RIJK b) => a.r * b.r + IJK.Dot(a.ijk, b.ijk);
		public static RIJK operator *(RIJK a, RIJK b) =>
			(Dot(a, b), a.r * b.ijk + b.r * a.ijk + IJK.Cross(a.ijk, b.ijk));
		public static HRIJK operator *(Reciprocal a, RIJK b) => new HRIJK(a, b);
		public RIJK Conjugate => (r, -i, -j, -k);
	}
	/// <summary>
	/// Dual Quaternion: w + xi + ij + zk + Wε + Xεi + Yεj + Zεk
	/// </summary>
	public readonly struct RIJKD {
		public readonly R r;
		public readonly I i;
		public readonly J j;
		public readonly K k;
		public readonly D d;
		public readonly DI di;
		public readonly DJ dj;
		public readonly DK dk;
		public RIJK real => (r, i, j, k);
		public RIJK dual => ((R)d.d, (I)di.x, (J)dj.y, (K)dk.z);
		public override string ToString() =>
			$"{r}{i.x:+0.######;-0.######;+0}i{j.y:+0.######;-0.######;+0}j{k.z:+0.######;-0.######;+0}k" +
			$"{d.d:+0.######;-0.######;+0}ε{di.x:+0.######;-0.######;+0}εi{dj.y:+0.######;-0.######;+0}εj{dk.z:+0.######;-0.######;+0}εk";
		public RIJKD(R w, I x, J y, K z, D d, DI dx, DJ dy, DK dz)
			=> (r, i, j, k, this.d, di, dj, dk) = (w, x, y, z, d, dx, dy, dz);
		public static RIJKD FromRealDual((R r, I i, J j, K k) r, (R d, I di, J dj, K dk) d) =>
			new RIJKD(r.r, r.i, r.j, r.k, (D)d.d.w, (DI)d.di.x, (DJ)d.dj.y, (DK)d.dk.z);
		public static RIJKD FromRealDual(RIJK r, RIJK d) =>
			new RIJKD(r.r, r.i, r.j, r.k, (D)d.r.w, (DI)d.i.x, (DJ)d.j.y, (DK)d.k.z);
		public static implicit operator RIJKD((R r, I i, J j, K k, D d, DI di, DJ dj, DK dk) t) =>
			new RIJKD(t.r, t.i, t.j, t.k, t.d, t.di, t.dj, t.dk);

		public static RIJKD operator -(RIJKD a) =>
			(-a.r, -a.i, -a.j, -a.k,
			-a.d, -a.di, -a.dj, -a.dk);
		public static RIJKD operator +(RIJKD a, RIJKD b) =>
		   (a.r + b.r, a.i + b.i, a.j + b.j, a.k + b.k,
		   a.d + b.d, a.di + b.di, a.dj + b.dj, a.dk + b.dk);
		public static RIJKD operator -(RIJKD a, RIJKD b) =>
			(a.r - b.r, a.i - b.i, a.j - b.j, a.k - b.k,
			a.d - b.d, a.di - b.di, a.dj - b.dj, a.dk - b.dk);
		public static RIJKD operator *(R a, RIJKD b) =>
			(a * b.r, a * b.i, a * b.j, a * b.k,
			a * b.d, a * b.di, a * b.dj, a * b.dk);
		public static RIJKD operator *(RIJKD a, RIJKD b) {
			var ar = a.real;
			var ad = a.dual;
			var br = b.real;
			var bd = b.dual;
			return FromRealDual(ar * br, ar * bd + ad * br);
		}
		public static RIJKD From(RIJK rotation, Vector3 position) {
			return FromRealDual(rotation,
				(RIJK)((R)0,
				(I)(.5f * position.x),
				(J)(.5f * position.y),
				(K)(.5f * position.z)) * rotation);
		}
		public Vector3 Position {
			get {
				var t = 2 * dual * real.Conjugate;
				return new Vector3(t.i.x, t.j.y, t.k.z);
			}
		}
		public (D d, DI di, DJ dj, DK dk) PositionAsDIJK {
			get {
				var t = 2 * dual * real.Conjugate;
				return ((D)t.r.w, (DI)t.i.x, (DJ)t.j.y, (DK)t.k.z);
			}
		}
	}
}