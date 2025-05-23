## ✅ Yapılacaklar Listesi (Manual Takip)

### 🎯 Temel Başlangıç
- [x] GitHub reposu oluştur
- [x] Proje Unity 6 ile başlatıldı
- [x] `.gitignore` dosyası eklendi
- [x] `README.md` hazırlandı
- [x] `feature/grid-system` branch’i açıldı

### 🧱 Oyun Mekanikleri
- [X] Grid sistemi kur (X × Y boyutlu)
- [X] Renkli taş prefab’ı oluştur
- [X] Ana ve ara renkleri enum olarak tanımla
- [X] Taşlar birleştiğinde renk dönüşümü hesapla
- [X] Max 2 taş seçilebilsin (seçim sistemi)
- [X] Geçersiz eşleşmede hata efekti
- [X] Geçerli eşleşmede taşları birleştir ve efekt göster
- [ ] Undo butonu ve son hareketi geri al

### 🧩 Level Sistemi
- [ ] İlk 3 level statik olarak hazırlanmalı
- [ ] Her level’da taş sayısı ve rengi ayarlanabilir olmalı
- [ ] Tüm taşlar beyaz olunca level tamamlanmalı
- [ ] Seviye geçiş sistemi
- [ ] Level başlangıç animasyonu

### 💅 Efektler & UI
- [ ] Taş seçildiğinde büyüme animasyonu
- [ ] Birleşme efekti (ışık, patlama vb.)
- [ ] UI: Seçim sayısı göstergesi
- [ ] UI: Elmas sayacı
- [ ] UI: Undo butonu

### ⚙️ Ekstra
- [ ] Süre kaydı tut (tamamlama süresi için)
- [ ] Seviye verisini dışarıdan yönet (JSON veya ScriptableObject)
- [ ] Notion sayfası bağlantısı README’ye eklendi




| Sınıf/Dosya Adı                | Görevi                                                                           |
| ------------------------------ | -------------------------------------------------------------------------------- |
| `Tile`                         | Her bir taşın (ve katmanlarının) davranışlarını yönetir.                         |
| `TileSpawner`                  | Grid üzerine tile'ları spawn eder, başlangıçta çağrılır.                         |
| `TileManager` *(isteğe bağlı)* | Oyun boyunca tile’lar ile ilgili genel yönetimi yapar (örn. hepsi bitti mi vs.). |



| Sınıf Adı            | Sorumlulukları / Görevleri                                           | Notlar / Bağlantılar                                                |
| -------------------- | -------------------------------------------------------------------- | ------------------------------------------------------------------- |
| **Tile**             | - Her bir karo (tile) için renk yığını (stack) tutar.                | Renkleri gösterir, seçime cevap verir, renk günceller.              |
|                      | - Renk yığınına renk ekleme/çıkarma (push/pop) işlemleri yapar.      | Inspector’da sprite veya materyalin rengini ayarlar.                |
|                      | - Kendi koordinatlarını (X, Y) tutar.                                | GridManager tarafından set edilir.                                  |
|                      | - Kullanıcı tıklaması ile SelectionManager’a bildirir.               |                                                                     |
| -----------------    | -------------------------------------------------------------------- | -------------------------------------------------------             |
| **GridManager**      | - Oyun alanını grid (2D dizi) olarak oluşturur.                      | Tile prefab’larını instantiate eder ve yerleştirir.                 |
|                      | - Tüm tile’lara renk yığınlarını oluşturup atar.                     | Renklerin dengeli dağılımını sağlar.                                |
| -----------------    | -------------------------------------------------------------------- | -------------------------------------------------------             |
| **ColorDataSO**      | - Renk verilerini (R, G, B ve isim) ScriptableObject olarak tutar.   | Veri olarak renk bilgisi sağlar.                                    |
|                      | - Unity’nin Color tipine dönüşüm fonksiyonu vardır.                  | ScriptableObject sayesinde düzenlenebilir.                          |
| -----------------    | -------------------------------------------------------------------- | -------------------------------------------------------             |
| **ColorManager**     | - Renk karışımı işlemlerini yapar (iki rengi toplama, kontrol).      | Base ve tüm renklerin listelerini tutar.                            |
|                      | - Renklerin toplamının beyaz olup olmadığını kontrol eder.           | Singleton olarak tüm projeden erişilebilir.                         |
|                      | - Rastgele temel renk seçer.                                         |                                                                     |
| -----------------    | -------------------------------------------------------------------- | -------------------------------------------------------             |
| **SelectionManager** | - Kullanıcının seçtiği tile’ları yönetir (en fazla 2).               | Singleton.                                                          |
|                      | - İki tile seçildiğinde renk kombinasyonunu dener.                   | Başarılı ise renkleri günceller, başarısızsa seçimi temizler.       |
| -----------------    | -------------------------------------------------------------------- | -------------------------------------------------------             |
| **Singleton<T>**     | - Generic bir singleton sınıfıdır.                                   | Diğer sınıflar bu sınıftan türeyerek tek örnek (instance) kullanır. |


