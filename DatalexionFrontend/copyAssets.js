const fs = require("fs");
const path = require("path");

try {
  const sourcePath = path.join(
    __dirname,
    "src/assets/images/marker-icon-completed.png"
  );
  const destinationPath = path.join(
    __dirname,
    "node_modules/leaflet/dist/images/marker-icon-completed.png"
  );

  fs.copyFile(sourcePath, destinationPath, (err) => {
    if (err) throw err;
    console.log("Archivo de imagen copiado exitosamente.");
  });
} catch (error) {
  console.error("Error al copiar el archivo:", error);
}
