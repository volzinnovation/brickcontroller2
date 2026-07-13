#!/usr/bin/env python3
"""Render the plain, two-page A4 OMBR executive summary from Markdown."""

from pathlib import Path
import re
from xml.sax.saxutils import escape

from reportlab.lib import colors
from reportlab.lib.pagesizes import A4
from reportlab.lib.styles import ParagraphStyle
from reportlab.lib.units import mm
from reportlab.pdfgen.canvas import Canvas
from reportlab.platypus import (
    HRFlowable,
    PageBreak,
    Paragraph,
    SimpleDocTemplate,
    Spacer,
)


PAGE_W, PAGE_H = A4
INK = colors.HexColor("#182433")
MUTED = colors.HexColor("#526274")
ACCENT = colors.HexColor("#168F86")
RULE = colors.HexColor("#CDD6DF")


STYLES = {
    "title": ParagraphStyle(
        "title",
        fontName="Helvetica-Bold",
        fontSize=22,
        leading=25,
        textColor=INK,
        spaceAfter=3,
    ),
    "subtitle": ParagraphStyle(
        "subtitle",
        fontName="Helvetica",
        fontSize=10.5,
        leading=13,
        textColor=MUTED,
        spaceAfter=8,
    ),
    "meta": ParagraphStyle(
        "meta",
        fontName="Helvetica",
        fontSize=7.8,
        leading=9.7,
        textColor=MUTED,
        leftIndent=10,
        firstLineIndent=-8,
        spaceAfter=1,
    ),
    "section": ParagraphStyle(
        "section",
        fontName="Helvetica-Bold",
        fontSize=12.2,
        leading=14.5,
        textColor=INK,
        spaceBefore=8,
        spaceAfter=3,
        keepWithNext=True,
    ),
    "body": ParagraphStyle(
        "body",
        fontName="Helvetica",
        fontSize=9,
        leading=11.4,
        textColor=INK,
        spaceAfter=4.2,
    ),
    "bullet": ParagraphStyle(
        "bullet",
        fontName="Helvetica",
        fontSize=8.75,
        leading=11,
        textColor=INK,
        leftIndent=12,
        firstLineIndent=-8,
        spaceAfter=2.2,
    ),
    "note": ParagraphStyle(
        "note",
        fontName="Helvetica-Oblique",
        fontSize=7.8,
        leading=9.8,
        textColor=MUTED,
        spaceBefore=5,
    ),
}


def inline_markup(text: str) -> str:
    """Convert the small Markdown subset used by the summary to ReportLab XML."""
    text = re.sub(r"\[([^\]]+)\]\([^)]+\)", r"\1", text)
    text = escape(text)
    text = re.sub(r"\*\*(.+?)\*\*", r"<b>\1</b>", text)
    text = re.sub(r"`(.+?)`", r'<font name="Courier">\1</font>', text)
    return text


def parse_body(lines: list[str]) -> list[tuple[str, str]]:
    blocks: list[tuple[str, str]] = []
    i = 0
    while i < len(lines):
        if not lines[i].strip():
            i += 1
            continue
        if lines[i].startswith("- "):
            parts = [lines[i][2:].strip()]
            i += 1
            while i < len(lines) and lines[i].strip() and not lines[i].startswith("- "):
                parts.append(lines[i].strip())
                i += 1
            blocks.append(("bullet", " ".join(parts)))
            continue
        parts = [lines[i].strip()]
        i += 1
        while i < len(lines) and lines[i].strip() and not lines[i].startswith("- "):
            parts.append(lines[i].strip())
            i += 1
        blocks.append(("paragraph", " ".join(parts)))
    return blocks


def parse_summary(source: Path):
    lines = source.read_text().splitlines()
    lines = [line for line in lines if not line.startswith("<!--")]

    title = next(line[2:].strip() for line in lines if line.startswith("# "))
    subtitle = next(line[2:].strip() for line in lines if line.startswith("> "))
    first_section = next(i for i, line in enumerate(lines) if line.startswith("## "))

    metadata = []
    for line in lines[:first_section]:
        if line.startswith("- "):
            metadata.append(line[2:].strip())

    sections = []
    i = first_section
    while i < len(lines):
        if not lines[i].startswith("## "):
            i += 1
            continue
        heading = lines[i][3:].strip()
        i += 1
        body_start = i
        while i < len(lines) and not lines[i].startswith("## "):
            i += 1
        sections.append((heading, parse_body(lines[body_start:i])))
    return title, subtitle, metadata, sections


def page_decor(canvas: Canvas, doc):
    canvas.saveState()
    canvas.setFillColor(colors.white)
    canvas.rect(0, 0, PAGE_W, PAGE_H, fill=1, stroke=0)
    canvas.setStrokeColor(RULE)
    canvas.setLineWidth(0.5)
    canvas.line(19 * mm, PAGE_H - 12 * mm, PAGE_W - 19 * mm, PAGE_H - 12 * mm)
    canvas.line(19 * mm, 13 * mm, PAGE_W - 19 * mm, 13 * mm)

    canvas.setFont("Helvetica-Bold", 6.7)
    canvas.setFillColor(MUTED)
    canvas.drawString(19 * mm, PAGE_H - 9.5 * mm, "OMBR EXECUTIVE SUMMARY")
    canvas.setFont("Helvetica", 6.7)
    canvas.drawRightString(PAGE_W - 19 * mm, PAGE_H - 9.5 * mm, "0.1 PROPOSAL | 13 JULY 2026")
    canvas.drawString(19 * mm, 9.5 * mm, "github.com/volzinnovation/brickcontroller2")
    canvas.setFont("Helvetica-Bold", 6.7)
    canvas.drawRightString(PAGE_W - 19 * mm, 9.5 * mm, f"{doc.page}/2")
    canvas.restoreState()


def build_story(source: Path):
    title, subtitle, metadata, sections = parse_summary(source)
    story = [
        Paragraph(inline_markup(title), STYLES["title"]),
        Paragraph(inline_markup(subtitle), STYLES["subtitle"]),
    ]
    for line in metadata:
        story.append(Paragraph("- " + inline_markup(line), STYLES["meta"]))
    story.extend(
        [
            Spacer(1, 4),
            HRFlowable(width="100%", thickness=1.2, color=ACCENT, spaceBefore=1, spaceAfter=3),
        ]
    )

    for heading, blocks in sections:
        if heading == "Implementation strategy":
            story.append(PageBreak())
        story.append(Paragraph(inline_markup(heading), STYLES["section"]))
        for kind, text in blocks:
            if kind == "bullet":
                story.append(Paragraph("- " + inline_markup(text), STYLES["bullet"]))
            else:
                style = STYLES["note"] if text.startswith("This summary is informative.") else STYLES["body"]
                story.append(Paragraph(inline_markup(text), style))
    return story


def render(source: Path, output: Path):
    output.parent.mkdir(parents=True, exist_ok=True)
    doc = SimpleDocTemplate(
        str(output),
        pagesize=A4,
        leftMargin=19 * mm,
        rightMargin=19 * mm,
        topMargin=16 * mm,
        bottomMargin=17 * mm,
        title="OMBR Executive Summary",
        author="Open Modular Brick Robotics project",
        subject="Plain two-page A4 executive summary for the OMBR 0.1 proposal",
        creator="OMBR ReportLab renderer",
        pageCompression=1,
    )
    doc.build(build_story(source), onFirstPage=page_decor, onLaterPages=page_decor)


if __name__ == "__main__":
    repo_root = Path(__file__).resolve().parents[4]
    source_path = repo_root / "spec" / "ombr" / "0.1" / "OMBR-EXECUTIVE-SUMMARY.md"
    output_path = repo_root / "output" / "pdf" / "OMBR-Executive-Summary.pdf"
    render(source_path, output_path)
    print(output_path)
