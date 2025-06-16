
# ColorFuse Projesi - Yapılacaklar Listesi

---

## ✅ Mevcut Durum
- [x] TileSelectionHandler ile seçim yönetimi hazır  
- [x] FusionProcessor ile renk birleştirme ve geçersizlik kontrolü var  
- [x] CameraShaker sallanma efekti mevcut  
- [x] Tile sınıfında görselleştirme ve DOTween animasyonları entegre  
- [x] LevelButton ve LevelMenuPanel ile seviye seçim arayüzü çalışıyor  
- [x] Async/await mantığı sistem geneline yayılmış durumda  

---

## 🔲 Yapılacaklar

### 🎮 Oyun Mekanikleri
- [ ] Undo sistemini yaz (hamleleri geri alma)  
- [ ] Restart butonuna seviye reset özelliği ekle  
- [ ] Joker ile ek hamle verme sistemini tamamla  
- [ ] TransferEffectPool sınıfını yaz ve efektler için kullan  
- [ ] FusionProcessor içine farklı birleşme efektleri ekle (ör: beyaz olunca patlama animasyonu vs.)

### 🧠 Event & Sistem Yönetimi
- [ ] Tüm sistemlerde UnsubscribeEvents() eksiklerini tamamla  
- [ ] JokerButtonsControl içindeki butonlar event tetiklesin (UndoPressedEvent, RestartEvent, AddMovesEvent gibi)  
- [ ] LevelUnlockedEvent gibi UI güncelleyici event sistemi ekle  
- [ ] GameCore.RegisterSystem(this) ile tüm sistemleri merkezi yönetime dahil et

### 📲 UI / Kullanıcı Arayüzü
- [ ] JokerButtonsControl için görsel geri bildirimler ekle (buton tıklanınca renk, shake, disable/enable)  
- [ ] Seviye geçildiğinde yeni seviyeyi aç, kilitleri kaldır  
- [ ] “Seviye tamamlandı” paneli tasarla (WinPanel)  
- [ ] “Seviye başarısız” paneli tasarla (FailPanel)  
- [ ] PausePanel UI sistemi ile entegre çalışsın

### 🧪 Oyun Verisi ve Kayıt
- [ ] PlayerPrefsService yerine RuntimeDataManager gibi daha sağlam yapı kur  
- [ ] Seviye geçme, kilit açma, kullanılan jokerleri kayıt altına al  
- [ ] Kalan hareket sayısını JSON ile kaydet / yükle  
- [ ] İlk girişte tüm seviyeleri LevelDataLoader üzerinden yükle ve cache’le  

### ⚙️ Gelişmiş Özellikler (Orta/Uzun Vadeli)
- [ ] DOTween ayarlarını merkezi yönetecek AnimationConfig sınıfı oluştur  
- [ ] VisualFeedbackService ile highlight, efekt, shake vb. animasyonları tek noktadan yönet  
- [ ] IGameState tabanlı WinState, FailState, PauseState gibi durumları uygula  
- [ ] Seviye hedefi veya görev sistemi ekle (ör: "Tüm kırmızılar birleşsin")  
- [ ] Hata logları için DebugService (event bazlı veya sistem tag’li) geliştir  

---

## 🧭 Önerilen Geliştirme Sırası
1. 🎮 Joker ve Undo mekaniklerini aktif hâle getir  
2. 🔄 Event unsubscribe temizliklerini tamamla  
3. 📲 UI panelleri ve level geçiş mantığını bağla  
4. 🧠 Kayıt sistemi (save/load) ile seviye ilerlemesini kaydet  
5. ⚙️ Gelişmiş animasyonlar ve efekt yönetimi için servis mimarisi kur

---

*Bu liste proje ilerledikçe güncellenecektir.*
