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

