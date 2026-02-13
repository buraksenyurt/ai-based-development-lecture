const express = require('express');
const path = require('path');

const app = express();
const PORT = 7000;

// Statik dosyaları sunmak için middleware
app.use(express.static(__dirname));

// Ana sayfa route'u
app.get('/', (req, res) => {
    res.sendFile(path.join(__dirname, 'cv.html'));
});

// Sunucuyu başlat
app.listen(PORT, () => {
    console.log(`🚀 Web sunucusu başlatıldı!`);
    console.log(`📄 CV sayfasını görüntülemek için: http://localhost:${PORT}`);
    console.log(`⏹️  Durdurmak için: Ctrl + C`);
});
